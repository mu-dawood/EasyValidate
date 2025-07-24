using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers
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
/// public class PropertyOnlyAttribute : Attribute, IValidationAttribute<string>
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
/// public class PropertyAndFieldAttribute : Attribute, IValidationAttribute<object>
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
/// public class FieldOnlyAttribute : Attribute, IValidationAttribute<int>
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
/// public class ParameterOnlyAttribute : Attribute, IValidationAttribute<string>
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
/// public class WrongAttribute1 : Attribute, IValidationAttribute<string>
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
/// public class WrongAttribute2 : Attribute, IValidationAttribute<string>
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
/// public class WrongAttribute3 : IValidationAttribute<string>
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
public class ValidationAttributeAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor MustInheritFromAttribute = new(
        id: ErrorIds.MustInheritFromAttribute,
        title: "Validation attribute must inherit from System.Attribute",
        messageFormat: "Class '{0}' implements IValidationAttribute but does not inherit from System.Attribute",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    private static readonly DiagnosticDescriptor MustHaveProperAttribute = new(
        id: ErrorIds.MustHaveProperAttributeUsage,
        title: "Validation attribute must have proper AttributeUsage",
        messageFormat: "Class '{0}' must have [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)] or similar with only Property, Field, and/or Parameter targets",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        [MustInheritFromAttribute, MustHaveProperAttribute];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(AnalyzeClass, SymbolKind.NamedType);
    }

    private void AnalyzeClass(SymbolAnalysisContext context)
    {
        var classSymbol = (INamedTypeSymbol)context.Symbol;

        if (classSymbol.TypeKind != TypeKind.Class || classSymbol.IsAbstract)
            return;

        // Check if the class ultimately implements IValidationAttribute interface
        bool isValidationAttribute = classSymbol.IsValidationAttribute();
        if (!isValidationAttribute)
            return;

        // Check if the class inherits from System.Attribute
        bool inheritsFromAttribute = InheritsFromSystemAttribute(classSymbol);
        if (!inheritsFromAttribute)
        {
            var diagnostic = Diagnostic.Create(MustInheritFromAttribute, classSymbol.Locations[0], classSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }
        
        // Check if the class has proper AttributeUsage
        bool hasProperAttributeUsage = HasProperAttributeUsage(classSymbol);
        if (!hasProperAttributeUsage)
        {
            var diagnostic = Diagnostic.Create(MustHaveProperAttribute, classSymbol.Locations[0], classSymbol.Name);
            context.ReportDiagnostic(diagnostic);
        }

        // Since we're using IValidationAttribute<T> interface, the compiler already enforces
        // that the Validate method is implemented correctly. We don't need to check for it manually.
    }

    private static bool InheritsFromSystemAttribute(INamedTypeSymbol classSymbol)
    {
        var currentType = classSymbol.BaseType;
        while (currentType != null)
        {
            if (currentType.ToDisplayString() == "System.Attribute")
                return true;
            currentType = currentType.BaseType;
        }
        return false;
    }

    private static bool HasProperAttributeUsage(INamedTypeSymbol classSymbol)
    {
        // Check the inheritance chain for AttributeUsage
        var currentType = classSymbol;
        while (currentType != null && currentType.Name != "Object")
        {
            var attributeUsageAttribute = currentType.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.ToDisplayString() == "System.AttributeUsageAttribute");

            if (attributeUsageAttribute != null)
            {
                return IsValidAttributeUsage(attributeUsageAttribute);
            }
            
            currentType = currentType.BaseType;
        }

        return false;
    }

    private static bool IsValidAttributeUsage(AttributeData attributeUsageAttribute)
    {
        // Check if we have valid targets argument
        var validTargetsArg = attributeUsageAttribute.ConstructorArguments.FirstOrDefault();
        if (validTargetsArg.Value == null)
            return false;

        var targetValue = (AttributeTargets)validTargetsArg.Value;
        
        // Check if it contains Property, Field, or Parameter
        bool hasPropertyFieldOrParameter = targetValue.HasFlag(AttributeTargets.Property) || 
                                         targetValue.HasFlag(AttributeTargets.Field) || 
                                         targetValue.HasFlag(AttributeTargets.Parameter);
        
        if (!hasPropertyFieldOrParameter)
            return false;
        
        // Check if it only contains Property, Field, and/or Parameter (no other targets)
        var allowedTargets = AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter;
        bool onlyHasAllowedTargets = (targetValue & ~allowedTargets) == 0;
        
        if (!onlyHasAllowedTargets)
            return false;

        // Check if AllowMultiple is set to true (or not specified, which defaults to false)
        var allowMultipleArg = attributeUsageAttribute.NamedArguments
            .FirstOrDefault(arg => arg.Key == "AllowMultiple");
        
        bool allowMultiple = allowMultipleArg.Value.Value is true;
        
        return allowMultiple;
    }
}
}
