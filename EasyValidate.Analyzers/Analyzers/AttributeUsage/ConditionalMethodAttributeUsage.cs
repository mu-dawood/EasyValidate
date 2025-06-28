using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers.AttributeUsage
{
    /// <summary>
    /// Validates that conditional methods specified in validation attributes exist and have correct signatures.
    /// </summary>
    /// <docs-explanation>
    /// When a validation attribute specifies a ConditionalMethod property, this analyzer ensures that:
    /// 1. The method exists on the class containing the validated property
    /// 2. The method accepts exactly one parameter of type IValidationResult
    /// 3. The method returns a boolean value
    /// 4. The method is accessible (public, private, protected, or internal)
    /// </docs-explanation>
    /// <docs-good-example>
    /// public class User
    /// {
    ///     [Required] { ConditionalMethod = "ShouldValidateEmail" }
    ///     public string Email { get; set; }
    ///     
    ///     private bool ShouldValidateEmail(IValidationResult result) => result.IsValid("Name");
    ///     public string Name { get; set; }
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// public class User
    /// {
    ///     [Required] { ConditionalMethod = "NonExistentMethod" }  // Method doesn't exist
    ///     public string Email { get; set; }
    ///     
    ///     public bool WrongSignature() { return true; }  // Missing IValidationResult parameter
    ///     public bool WrongParameter(int param) { return true; }  // Wrong parameter type
    ///     public string WrongReturnType(IValidationResult result) => "test";  // Wrong return type
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// Create the missing conditional method|Ensure method accepts IValidationResult parameter|Ensure method returns bool|Remove ConditionalMethod property if not needed
    /// </docs-fixes>
    public class ConditionalMethodAttributeUsage : IAttributeUsageProcessor
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

        public ICollection<DiagnosticDescriptor> DiagnosticDescriptors => new[] { Rule, InvalidStrategyRule, MissingConditionalMethodRule };

        public void Process(SymbolAnalysisContext context, AttributeInfo attribute, ITypeSymbol memberType, string memberName)
        {
            var attributeClass = attribute.AttributeClass;
            if (attributeClass == null)
                return; // Not a validation attribute, so no check needed

            // Check if ConditionalMethod is specified in named arguments
            var conditionalMethodName = attribute.NamedArguments.GetArgumentValue<string>("ConditionalMethod");
            var strategy = attribute.NamedArguments.GetArgumentValue<int>("Strategy");
            // 0: Default, 1: ConditionalAndStopChain, 2: ConditionalAndContinue
            var isConditionalStrategy = strategy == 1 || strategy == 2;

            if (!string.IsNullOrEmpty(conditionalMethodName))
            {
                if (!isConditionalStrategy)
                {
                    // ConditionalMethod is set but strategy is not conditional
                    ReportDiagnostic(context, attribute, InvalidStrategyRule, conditionalMethodName!, strategy.ToString());
                    return;
                }
            }
            else if (isConditionalStrategy)
            {
                // Strategy is conditional but ConditionalMethod is not set
                ReportDiagnostic(context, attribute, MissingConditionalMethodRule, strategy.ToString());
                return;
            }
            else
            {
                // No conditional method specified, nothing to validate
                return;
            }

            // Find the containing class
            var containingClass = GetContainingClass(context);
            if (containingClass == null)
                return; // Can't find containing class, skip validation

            // Look for the conditional method
            var conditionalMethod = conditionalMethodName != null
                ? containingClass.GetMembers(conditionalMethodName)
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault()
                : null;

            if (conditionalMethod == null)
            {
                // Method doesn't exist
                ReportDiagnostic(context, attribute, Rule, conditionalMethodName ?? "unknown", $"does not exist on class '{containingClass.Name}'");
                return;
            }

            // Check method signature
            if (conditionalMethod.Parameters.Length != 1)
            {
                // Method must have exactly one parameter
                ReportDiagnostic(context, attribute, Rule, conditionalMethodName ?? "unknown", "must accept exactly one parameter of type IValidationResult");
                return;
            }

            // Check if the parameter is of type IValidationResult
            var parameter = conditionalMethod.Parameters[0];
            var parameterType = parameter.Type;

            // Check if the parameter type is IValidationResult
            if (!IsIValidationResult(parameterType))
            {
                ReportDiagnostic(context, attribute, Rule, conditionalMethodName ?? "unknown", "must accept a parameter of type IValidationResult");
                return;
            }

            if (conditionalMethod.ReturnType.SpecialType != SpecialType.System_Boolean)
            {
                // Method doesn't return bool
                ReportDiagnostic(context, attribute, Rule, conditionalMethodName ?? "unknown", "must return bool");
                return;
            }

            // All checks passed - no diagnostic needed
        }

        private INamedTypeSymbol? GetContainingClass(SymbolAnalysisContext context)
        {
            // Get the property symbol from the context
            if (context.Symbol is IPropertySymbol propertySymbol)
            {
                return propertySymbol.ContainingType;
            }

            // If not a property, try to get from field
            if (context.Symbol is IFieldSymbol fieldSymbol)
            {
                return fieldSymbol.ContainingType;
            }

            return null;
        }

        private bool IsIValidationResult(ITypeSymbol typeSymbol)
        {
            // Check if the type is exactly IValidationResult
            if (typeSymbol.TypeKind == TypeKind.Interface &&
                typeSymbol.Name == "IValidationResult" &&
                typeSymbol.ContainingNamespace?.ToDisplayString() == "EasyValidate.Core.Abstraction")
            {
                return true;
            }

            // Check if the type implements IValidationResult
            return typeSymbol.AllInterfaces.Any(i =>
                i.Name == "IValidationResult" &&
                i.ContainingNamespace?.ToDisplayString() == "EasyValidate.Core.Abstraction");
        }

        private void ReportDiagnostic(SymbolAnalysisContext context, AttributeInfo attribute, DiagnosticDescriptor rule, params object[] args)
        {
            var diagnostic = Diagnostic.Create(
                rule,
                attribute.Location,
                args);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
