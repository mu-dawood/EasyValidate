using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

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
    internal class ConditionalMethodAttributeUsage : AttributeAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new(
            id: ErrorIds.ConditionalMethodError,
            title: "Conditional method validation error",
            messageFormat: "Conditional method '{0}' {1}",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "A validation attribute specifies a conditional method that either doesn't exist, has wrong parameters, or doesn't return bool.");

        private static readonly DiagnosticDescriptor InvalidStrategyRule = new(
            id: ErrorIds.ConditionalMethodInvalidStrategyError,
            title: "Conditional method strategy error",
            messageFormat: "Conditional method '{0}' requires ExecutionStrategy.ConditionalAndStopChain or ExecutionStrategy.ConditionalAndContinue, but found '{1}'",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "A conditional method was specified but the strategy is not a valid conditional execution strategy.");

        private static readonly DiagnosticDescriptor MissingConditionalMethodRule = new(
            id: ErrorIds.ConditionalMethodMissingError,
            title: "Conditional method missing error",
            messageFormat: "ExecutionStrategy '{0}' requires a non-empty ConditionalMethod name",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "A conditional execution strategy was specified but no conditional method name was provided.");
        private static readonly DiagnosticDescriptor InvalidNameRule = new(
            id: ErrorIds.ConditionalMethodInvalidNameError,
            title: "Conditional method name is invalid",
            messageFormat: "Conditional method name '{0}' is not a valid C# method name",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "The ConditionalMethod property must be a valid C# method name without whitespace or invalid characters.");

        internal override ICollection<DiagnosticDescriptor> DiagnosticDescriptors => [Rule, InvalidStrategyRule, MissingConditionalMethodRule, InvalidNameRule];

        internal override bool Analyze(AnalyserContext context, AttributeInfo attribute)
        {
            var attributeClass = attribute.Attribute.AttributeClass;
            if (attributeClass == null)
                return true; // Not a validation attribute, so no check needed

            // Check if ConditionalMethod is specified in named arguments
            var conditionalMethodName = attribute.Attribute.NamedArguments.GetArgumentValue<string>("ConditionalMethod");
            var isValid = string.IsNullOrEmpty(conditionalMethodName) || Regex.IsMatch(conditionalMethodName, @"^[_a-zA-Z][_a-zA-Z0-9]*$");
            if (!isValid)
            {
                ReportDiagnostic(context, attribute.Location, InvalidNameRule, conditionalMethodName!);
                return false;
            }
            var strategy = attribute.Attribute.NamedArguments.GetArgumentValue<int>("Strategy");
            // 0: Default, 1: ConditionalAndStopChain, 2: ConditionalAndContinue
            var isConditionalStrategy = strategy == 1 || strategy == 2;

            if (!string.IsNullOrEmpty(conditionalMethodName))
            {
                if (!isConditionalStrategy)
                {
                    // ConditionalMethod is set but strategy is not conditional
                    ReportDiagnostic(context, attribute.Location, InvalidStrategyRule, conditionalMethodName!, strategy.ToString());
                    return false;
                }
            }
            else if (isConditionalStrategy)
            {
                // Strategy is conditional but ConditionalMethod is not set
                ReportDiagnostic(context, attribute.Location, MissingConditionalMethodRule, strategy.ToString());
                return false;
            }
            else
            {
                // No conditional method specified, nothing to validate
                return true;
            }

            // Find the containing class
            var containingClass = context.ContainingClass;
            if (containingClass == null)
                return false; // Can't find containing class, skip validation

            // Look for the conditional method
            var conditionalMethod = conditionalMethodName != null
                ? containingClass.GetMembers(conditionalMethodName)
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault()
                : null;

            if (conditionalMethod == null)
            {
                // Method doesn't exist
                ReportDiagnostic(context, attribute.Location, Rule, conditionalMethodName ?? "unknown", $"does not exist on class '{containingClass.Name}'");
                return false;
            }

            // Check method signature
            if (conditionalMethod.Parameters.Length != 1)
            {
                // Method must have exactly one parameter
                ReportDiagnostic(context, attribute.Location, Rule, conditionalMethodName ?? "unknown", "must accept exactly one parameter of type IChainResult");
                return false;
            }

            // Check if the parameter is of type IChainResult
            var parameter = conditionalMethod.Parameters[0];
            var parameterType = parameter.Type;

            // Check if the parameter type is IChainResult
            if (!parameterType.InheritsFrom("EasyValidate.Abstractions.IChainResult"))
            {
                ReportDiagnostic(context, attribute.Location, Rule, conditionalMethodName ?? "unknown", "must accept a parameter of type IChainResult");
                return false;
            }

            // Allow method to return bool, or ValueTask<bool>
            var returnType = conditionalMethod.ReturnType;
            var isBoolReturn = returnType.SpecialType == SpecialType.System_Boolean;
            var isAwaitableBoolReturn = false;
            var (isAsync, arguments) = conditionalMethod.IsAsyncMethod();
            if (returnType.GetFullName().Contains("System.Threading.Tasks.ValueTask") && isAsync && arguments.Length == 1 && arguments[0].SpecialType == SpecialType.System_Boolean)
            {
                isAwaitableBoolReturn = true;
            }
            if (!isBoolReturn && !isAwaitableBoolReturn)
            {
                // Method doesn't return bool or a true awaitable type with bool result
                ReportDiagnostic(context, attribute.Location, Rule, conditionalMethodName ?? "unknown", "must return bool or ValueTask<bool>");
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
