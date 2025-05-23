using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ValidationMethodAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "VAL001";
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Invalid Validate method in derived class",
        messageFormat: "Class '{0}' must have at least one 'Validate(string, Type)' method with exactly two parameters, and all 'Validate' methods must have exactly two parameters. Ensure all 'Validate' methods return 'EasyValidate.Abstraction.AttributeResult'.",
        category: "Design",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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

        // Check if the class ultimately derives from ValidationAttributeBase (by full name)
        var currentBase = classSymbol.BaseType;
        bool isValidationAttribute = false;
        while (currentBase != null && currentBase.Name != "Object")
        {
            if (currentBase.ToDisplayString() == "EasyValidate.Abstraction.Attributes.ValidationAttributeBase")
            {
                isValidationAttribute = true;
                break;
            }
            currentBase = currentBase.BaseType;
        }
        if (!isValidationAttribute)
            return;

        // Search for Validate methods in the class and its base types
        var validateMethods = new List<IMethodSymbol>();
        var currentType = classSymbol;
        while (currentType != null && currentType.Name != "Object")
        {
            validateMethods.AddRange(currentType
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.Name == "Validate" && !m.IsStatic));
            currentType = currentType.BaseType;
        }

        bool hasCorrectOverload = validateMethods.Any(m =>
            m.Parameters.Length == 2 &&
            m.Parameters[0].Type.SpecialType == SpecialType.System_String);

        bool allHaveTwoParameters = validateMethods.All(m => m.Parameters.Length == 2);

        bool allReturnCorrectType = validateMethods.All(m =>
            m.ReturnType.ToDisplayString() == "EasyValidate.Abstraction.AttributeResult");

        if (!hasCorrectOverload || !allHaveTwoParameters || !allReturnCorrectType)
        {
            var diagnostic = Diagnostic.Create(Rule, classSymbol.Locations[0], classSymbol.Name);
           context.ReportDiagnostic(diagnostic);
        }
    }
}
