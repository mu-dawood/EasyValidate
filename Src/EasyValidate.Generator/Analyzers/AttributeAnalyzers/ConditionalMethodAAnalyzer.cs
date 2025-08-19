using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyValidate.Generator.Analyzers.AttributeAnalyzers
{
    /// <summary>
    /// Validates that conditional methods specified in validation attributes exist and have correct signatures.
    /// </summary>
    /// <docs-explanation>
    /// When a validation attribute specifies a ConditionalMethod property, this analyzer ensures that:
    /// 1. The method exists on the class containing the validated property
    /// 2. The method accepts exactly one parameter of type IValidationResult
    /// 3. The method returns a boolean value
    /// 4. The method is accessible (internal, private, protected, or internal)
    /// </docs-explanation>
    /// <docs-good-example>
    /// internal class User
    /// {
    ///     [Required] { ConditionalMethod = "ShouldValidateEmail" }
    ///     internal string Email { get; set; }
    ///     
    ///     private bool ShouldValidateEmail(IValidationResult result) => result.IsValid("Name");
    ///     internal string Name { get; set; }
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// internal class User
    /// {
    ///     [Required] { ConditionalMethod = "NonExistentMethod" }  // Method doesn't exist
    ///     internal string Email { get; set; }
    ///     
    ///     internal bool WrongSignature() { return true; }  // Missing IValidationResult parameter
    ///     internal bool WrongParameter(int param) { return true; }  // Wrong parameter type
    ///     internal string WrongReturnType(IValidationResult result) => "test";  // Wrong return type
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// Create the missing conditional method|Ensure method accepts IValidationResult parameter|Ensure method returns bool|Remove ConditionalMethod property if not needed
    /// </docs-fixes>
    internal class ConditionalMethodAAnalyzer : AttributeAnalyzer
    {
        private static readonly DiagnosticDescriptor MissingMethodRule = new(
            id: ErrorIds.ConditionalMethodIsMissing,
            title: "Conditional method is missing",
            messageFormat: "Conditional method '{0}' is missing on the containing class",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The ConditionalMethod property must point to an existing method on the containing class."
        );

        private static readonly DiagnosticDescriptor InvalidNameRule = new(
            id: ErrorIds.InvalidConditionalMethodName,
            title: "Conditional method name is invalid",
            messageFormat: "Conditional method name '{0}' is not a valid C# method name",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The ConditionalMethod property must be a valid C# method name without whitespace or invalid characters."
        );

        private static readonly DiagnosticDescriptor LengthRule = new(
            id: ErrorIds.ConditionalMethodInvalidParameterLength,
            title: "Invalid conditional method parameters",
            messageFormat: "Conditional method '{0}' must accept exactly one parameter of type IChainResult",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The conditional method must accept exactly one parameter of type IChainResult."
        );

        private static readonly DiagnosticDescriptor ParmterTypeMismatchRule = new(
            id: ErrorIds.ConditionalMethodFirstParameterTypeMismatch,
            title: "Conditional method parameter type mismatch",
            messageFormat: "Conditional method '{0}' first parameter must be of type IChainResult",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The first parameter of the conditional method must be of type IChainResult."
        );

        private static readonly DiagnosticDescriptor ReturnTypeRule = new(
            id: ErrorIds.ConditionalMethodReturnTypeMismatch,
            title: "Conditional method return type mismatch",
            messageFormat: "Conditional method '{0}' must return bool or ValueTask<bool>",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The conditional method must return a boolean value or a ValueTask<bool>."
        );
        internal override ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [
            MissingMethodRule,
            InvalidNameRule,
            LengthRule,
            ParmterTypeMismatchRule,
            ReturnTypeRule
        ];

        internal override bool Analyze(AnalyserContext context, AttributeInfo attribute)
        {
            var attributeClass = attribute.Attribute.AttributeClass;
            if (attributeClass == null)
                return true; // Not a validation attribute, so no check needed

            // Check if ConditionalMethod is specified in named arguments
            var conditionalMethodName = attribute.Attribute.NamedArguments.GetArgumentValue<string>("ConditionalMethod");
            if (conditionalMethodName == null)
                return true; // No conditional method specified, nothing to validate
            var isValid = !string.IsNullOrWhiteSpace(conditionalMethodName) && Regex.IsMatch(conditionalMethodName, @"^[_a-zA-Z][_a-zA-Z0-9]*$");
            if (!isValid)
            {
                ReportDiagnostic(context, attribute.Location, InvalidNameRule, conditionalMethodName!);
                return false;
            }

            // Find the containing class
            var containingClass = context.ContainingClass;
            if (containingClass == null)
                return false; // Can't find containing class, skip validation

            // Look for the conditional method
            var conditionalMethod = containingClass.GetMembers(conditionalMethodName)
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault();

            if (conditionalMethod == null)
            {
                // Method doesn't exist
                ReportDiagnostic(context, attribute.Location, MissingMethodRule, conditionalMethodName);
                return false;
            }

            // Check method signature
            if (conditionalMethod.Parameters.Length != 1)
            {
                // Method must have exactly one parameter
                ReportDiagnostic(context, conditionalMethod.Locations.FirstOrDefault() ?? attribute.Location, LengthRule, conditionalMethodName ?? "unknown");
                return false;
            }

            // Check if the parameter is of type IChainResult
            var parameter = conditionalMethod.Parameters[0];
            var parameterType = parameter.Type;

            // Check if the parameter type is IChainResult
            if (!parameterType.ImplementsInterface("EasyValidate.Abstractions.IChainResult", true))
            {
                ReportDiagnostic(context, parameter.Locations.FirstOrDefault() ?? attribute.Location, ParmterTypeMismatchRule, conditionalMethodName ?? "unknown");
                return false;
            }

            // Allow method to return bool, or ValueTask<bool>
            var returnType = conditionalMethod.ReturnType;
            var isBoolReturn = returnType.SpecialType == SpecialType.System_Boolean;
            var isAwaitableBoolReturn = false;
            var (isAsync, arguments) = conditionalMethod.IsAsyncMethod();
            if (returnType.GetFullName().StartsWith("global::System.Threading.Tasks.ValueTask") && isAsync && arguments.Length == 1 && arguments[0].SpecialType == SpecialType.System_Boolean)
            {
                isAwaitableBoolReturn = true;
            }
            if (!isBoolReturn && !isAwaitableBoolReturn)
            {
                var methodDecl = conditionalMethod.DeclaringSyntaxReferences
                .FirstOrDefault()?
                .GetSyntax() as MethodDeclarationSyntax;

                var location = methodDecl?.ReturnType.GetLocation();
                // Method doesn't return bool or a true awaitable type with bool result
                ReportDiagnostic(context, location ?? conditionalMethod.Locations.FirstOrDefault() ?? attribute.Location, ReturnTypeRule, conditionalMethodName ?? "unknown");
                return false;
            }
            return true; // All checks passed, conditional method is valid
            // All checks passed - no diagnostic needed
        }





        private void ReportDiagnostic(AnalyserContext context, Location location, DiagnosticDescriptor rule, params object[] args)
        {
            var diagnostic = Diagnostic.Create(
                rule,
                location,
                args);
            context.Context.ReportDiagnostic(diagnostic);
        }
    }
}
