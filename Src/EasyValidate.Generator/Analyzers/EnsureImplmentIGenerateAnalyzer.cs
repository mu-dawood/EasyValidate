using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Generator.Analyzers
{
    /// <summary>
    /// Analyzer that validates the implementation of validation attributes by checking:
    /// inheritance from System.Attribute and proper AttributeUsage configuration.
    /// </summary>
    /// <docs-explanation>
    /// All validation attributes that implement IValidationAttribute must inherit from System.Attribute 
    /// and have AttributeUsage that targets only Property, Field, and/or Parameter with AllowMultiple = false. 
    /// The IValidationAttribute interface contract ensures the Validate method is properly implemented.
    /// This analyzer ensures proper implementation of custom validation attributes by performing 
    /// separate checks for each requirement and providing specific error messages.
    /// </docs-explanation>
    /// <docs-good-example>
    /// // Valid: Property only
    /// [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    /// public class PropertyOnlyAttribute : Attribute, IValidationAttribute&lt;string&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "PROP001";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(object obj, string propertyName, string value, Type propertyType) => true;
    /// }
    /// 
    /// // Valid: Property and Field
    /// [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    /// public class PropertyAndFieldAttribute : Attribute, IValidationAttribute&lt;object&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "BOTH001";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(object value, Type propertyType) => true;
    /// }
    /// 
    /// // Valid: Field only  
    /// [AttributeUsage(AttributeTargets.Field)]
    /// public class FieldOnlyAttribute : Attribute, IValidationAttribute&lt;int&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "FIELD001";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(int value, Type propertyType) => true;
    /// }
    /// 
    /// // Valid: Parameter only  
    /// [AttributeUsage(AttributeTargets.Parameter)]
    /// public class ParameterOnlyAttribute : Attribute, IValidationAttribute&lt;string&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "PARAM001";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(string value, Type propertyType) => true;
    /// }
    /// </docs-good-example>
    /// <docs-bad-example>
    /// // Missing AttributeUsage - triggers VAL002
    /// public class WrongAttribute1 : Attribute, IValidationAttribute&lt;string&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "WRONG001";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(string value, Type propertyType) => true;
    /// }
    /// 
    /// // Wrong AttributeUsage targets - triggers VAL002
    /// [AttributeUsage(AttributeTargets.Method)]
    /// public class WrongAttribute2 : Attribute, IValidationAttribute&lt;string&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "WRONG002";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(string value, Type propertyType) => true;
    /// }
    /// 
    /// // Does not inherit from Attribute - triggers VAL001
    /// public class WrongAttribute3 : IValidationAttribute&lt;string&gt;
    /// {
    ///     public NullableBehavior NullableBehavior { get; set; }
    ///     public string NullErrorMessage { get; set; } = "";
    ///     public string ErrorCode => "WRONG003";
    ///     public string ErrorMessage { get; set; } = "";
    ///     
    ///     public bool Validate(string value, Type propertyType) => true;
    /// }
    /// </docs-bad-example>
    /// <docs-fixes>
    /// VAL001: Inherit from System.Attribute|VAL002: Add proper AttributeUsage with Property/Field/Parameter targets and AllowMultiple = false
    /// </docs-fixes>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EnsureImplmentIGenerateAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// Diagnostic descriptor for missing required types in validation attribute usage.
        /// </summary>
        private readonly DiagnosticDescriptor MissingTypeDiagnostic = new(
            ErrorIds.ValidateAttributeUsageMissingType,
            "Missing Validation Type Error",
            "Class '{0}' is missing required type(s): [{1}]",
            "Usage",
            DiagnosticSeverity.Error,
            true);
        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
                [MissingTypeDiagnostic];

        /// <summary>
        /// Initializes the analyzer and registers symbol actions.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
        }

        /// <summary>
        /// Analyzes a class symbol to ensure it implements IGenerate when using validation attributes.
        /// Reports diagnostics if requirements are not met.
        /// </summary>
        /// <param name="context">The symbol analysis context.</param>
        private void AnalyzeClass(SymbolAnalysisContext context)
        {
            var classSymbol = (INamedTypeSymbol)context.Symbol;

            if (classSymbol.TypeKind != TypeKind.Class || classSymbol.IsAbstract)
                return;
            if (classSymbol.ImplementsIGenerate()) return;

            foreach (var member in classSymbol.GetMembers())
            {
                if (member is IPropertySymbol property)
                {
                    foreach (var attribute in property.GetAttributes())
                    {
                        if (attribute.AttributeClass.IsValidationAttribute())
                        {
                            ReportMissingTypeDiagnostic(context, classSymbol, "interface:EasyValidate.Abstractions.IGenerate");
                            return; // Stop further analysis for this property
                        }
                    }
                }
                else if (member is IFieldSymbol field)
                {
                    foreach (var attribute in field.GetAttributes())
                    {
                        if (attribute.AttributeClass.IsValidationAttribute())
                        {
                            ReportMissingTypeDiagnostic(context, classSymbol, "interface:EasyValidate.Abstractions.IGenerate");
                            return; // Stop further analysis for this property
                        }
                    }
                }
                else if (member is IMethodSymbol method)
                {
                    foreach (var parmter in method.Parameters)
                    {
                        foreach (var attribute in parmter.GetAttributes())
                        {
                            if (attribute.AttributeClass.IsValidationAttribute())
                            {
                                ReportMissingTypeDiagnostic(context, classSymbol, "interface:EasyValidate.Abstractions.IGenerate");
                                return; // Stop further analysis for this method
                            }
                        }
                    }
                }
            }

        }
        /// <summary>
        /// Reports a diagnostic for missing required types on the given class symbol.
        /// </summary>
        /// <param name="context">The symbol analysis context.</param>
        /// <param name="classType">The class symbol being analyzed.</param>
        /// <param name="formattedTypes">The required types that are missing.</param>
        private void ReportMissingTypeDiagnostic(SymbolAnalysisContext context, INamedTypeSymbol classType, params string[] formattedTypes)
        {
            var diagnostic = Diagnostic.Create(
                MissingTypeDiagnostic,
                classType.Locations.FirstOrDefault(),
                classType.Name,
                string.Join(", ", formattedTypes));

            context.ReportDiagnostic(diagnostic);
        }

    }
}
