using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Generator.Analyzers
{
    /// <summary>
    /// Roslyn analyzer for validation attributes in EasyValidate.
    /// Ensures that custom validation attributes:
    /// <list type="bullet">
    /// <item>Inherit from <c>System.Attribute</c></item>
    /// <item>Implement the correct generic <c>IValidationAttribute&lt;T&gt;</c> or <c>IAsyncValidationAttribute&lt;T&gt;</c> interface</item>
    /// <item>Have a valid <c>AttributeUsage</c> targeting only Property, Field, and/or Parameter, with <c>AllowMultiple = true</c></item>
    /// <item>Follow rules for ValidationContext properties (public setter, unique type, correct type)</item>
    /// </list>
    /// Reports specific diagnostics for each violation and provides actionable error messages for code fix providers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This analyzer is essential for enforcing best practices and correctness in custom validation attribute implementations.
    /// It checks inheritance, interface implementation, attribute usage, and context property rules, helping maintain robust validation logic.
    /// </para>
    /// <para>
    /// <b>Common diagnostics:</b>
    /// <list type="table">
    /// <item><term>VAL001</term><description>Must inherit from System.Attribute</description></item>
    /// <item><term>VAL002</term><description>Must have proper AttributeUsage</description></item>
    /// <item><term>VAL003</term><description>ValidationContext property rule violation</description></item>
    /// <item><term>VAL004</term><description>Must implement generic IValidationAttribute&lt;T&gt; or IAsyncValidationAttribute&lt;T&gt;</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// <c>
    /// // Valid attribute
    /// [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    /// public class MyValidationAttribute : Attribute, IValidationAttribute&lt;string&gt;
    /// {
    ///     public bool Validate(string value, Type propertyType) =&gt; true;
    /// }
    /// </c>
    /// <c>
    /// // Invalid: missing AttributeUsage
    /// public class BadAttribute : Attribute, IValidationAttribute&lt;string&gt;
    /// {
    ///     public bool Validate(string value, Type propertyType) =&gt; true;
    /// }
    /// // Diagnostic: Must have proper AttributeUsage
    /// </c>
    /// <c>
    /// // Invalid: does not inherit from Attribute
    /// public class BadAttribute2 : IValidationAttribute&lt;string&gt;
    /// {
    ///     public bool Validate(string value, Type propertyType) =&gt; true;
    /// }
    /// // Diagnostic: Must inherit from System.Attribute
    /// </c>
    /// </para>
    /// </example>
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
    /// <summary>
    /// Gets the diagnostics supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            [MustInheritFromAttribute, MustHaveProperAttribute, ValidationContextPropertyDiagnostic, MustImplmentGeneric];

        /// <summary>
        /// Initializes the analyzer and registers symbol actions for class analysis.
        /// </summary>
        /// <param name="context">The analysis context.</param>
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
