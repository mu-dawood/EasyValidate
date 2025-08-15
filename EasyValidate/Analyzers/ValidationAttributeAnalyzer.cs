using System;
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

        private static readonly DiagnosticDescriptor ValidationContextPropertyDiagnostic = new(
            id: ErrorIds.ValidationContextPropertyDiagnostic,
            title: "ValidationContext property rule violation",
            messageFormat: "{0}",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
        private static readonly DiagnosticDescriptor MustHaveProperAttribute = new(
            id: ErrorIds.MustHaveProperAttributeUsage,
            title: "Validation attribute must have proper AttributeUsage",
            messageFormat: "Class '{0}' must have [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)] or similar with only Property, Field, and/or Parameter targets",
            category: "Design",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        private static readonly DiagnosticDescriptor MustImplmentGeneric = new(
                   id: ErrorIds.ValidateAttributeMustImplmentGeneric,
                   title: "Validation attribute must implement IValidationAttribute<T>",
                   messageFormat: "Class '{0}' must implement IValidationAttribute<T> or IAsyncValidationAttribute<T> where T is the type of the property it validates you can use also implement IValidationAttribute<TInput,TOutput> for transformations",
                   category: "Design",
                   defaultSeverity: DiagnosticSeverity.Error,
                   isEnabledByDefault: true
        );
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
                    [MustInheritFromAttribute, MustHaveProperAttribute, ValidationContextPropertyDiagnostic, MustImplmentGeneric];

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
            bool isValidationAttribute = classSymbol.IsValidationAttributeBase();
            if (!isValidationAttribute)
                return;

            if (!classSymbol.IsValidationAttribute())
            {
                var classSyntax = classSymbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax;
                var baseList = classSyntax?.BaseList;
                var interfaceType = baseList?.Types.FirstOrDefault(t => t.Type.ToString().StartsWith("IValidationAttribute"));
                var location = interfaceType?.GetLocation() ?? classSymbol.Locations[0];
                var diagnostic = Diagnostic.Create(MustImplmentGeneric, location, classSymbol.Name);
                context.ReportDiagnostic(diagnostic);
                
                return;
            }

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


            var props = classSymbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.Type.GetFullName() == "global::System.IServiceProvider" || p.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
                .ToList();


            // 1. Public setter check
            foreach (var prop in props)
            {
                var setter = prop.SetMethod;
                if (setter == null || !setter.DeclaredAccessibility.HasFlag(Accessibility.Public))
                {
                    var message = $"Property '{prop.Name}' decorated with ValidationContextAttribute must have a public setter.";
                    if (prop.Type.GetFullName() == "global::System.IServiceProvider")
                        message = $"Property '{prop.Name}' of type '{prop.Type.ToDisplayString()}' must have a public setter.";

                    var diag = Diagnostic.Create(
                        ValidationContextPropertyDiagnostic,
                        prop.Locations.FirstOrDefault() ?? classSymbol.Locations.FirstOrDefault(),
                        message);
                    context.ReportDiagnostic(diag);
                }
                // Check if property type is interface or class (not struct, enum, or primitive)
                var typeKind = prop.Type.TypeKind;
                var isString = prop.Type.SpecialType == SpecialType.System_String;
                if (isString || (typeKind != TypeKind.Interface && typeKind != TypeKind.Class))
                {
                    var message = $"Property '{prop.Name}' decorated with ValidationContextAttribute must be of an interface, class, or string type, not '{typeKind}'.";
                    var diag = Diagnostic.Create(
                        ValidationContextPropertyDiagnostic,
                        prop.Locations.FirstOrDefault() ?? classSymbol.Locations.FirstOrDefault(),
                        message);
                    context.ReportDiagnostic(diag);
                }
            }

            // 2. Unique type check
            var typeGroups = props.GroupBy(p => p.Type.ToDisplayString());
            foreach (var group in typeGroups)
            {
                if (group.Count() > 1)
                {
                    foreach (var prop in group)
                    {
                        var message = $"Multiple properties of type '{group.Key}' decorated with ValidationContextAttribute are not allowed. Only one per type is permitted.";
                        if (prop.Type.GetFullName() == "global::System.IServiceProvider")
                            message = $"Property '{prop.Name}' of type '{group.Key}' must be unique. Multiple properties of the same type are not allowed.";

                        var diag = Diagnostic.Create(
                            ValidationContextPropertyDiagnostic,
                            prop.Locations.FirstOrDefault() ?? classSymbol.Locations.FirstOrDefault(),
                            message);
                        context.ReportDiagnostic(diag);

                    }

                }
            }


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
