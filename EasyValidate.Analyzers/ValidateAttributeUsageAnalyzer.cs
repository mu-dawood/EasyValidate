using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ValidateAttributeUsageAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "VAL002";

        private static readonly DiagnosticDescriptor Rule = new(
            id: DiagnosticId,
            title: "Invalid Attribute Usage",
            messageFormat: "The attribute '{0}' is used incorrectly. Ensure it is applied to the correct target and follows the expected usage.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(AnalyzeAttributeUsage, SymbolKind.NamedType);
        }

        private void AnalyzeAttributeUsage(SymbolAnalysisContext context)
        {
            var classSymbol = (INamedTypeSymbol)context.Symbol;

            if (classSymbol.TypeKind != TypeKind.Class)
                return;

            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                foreach (var attribute in member.GetAttributes())
                {
                    var attributeClass = attribute.AttributeClass;
                    if (!IsValidationAttribute(attributeClass))
                        continue;

                    if (!IsCompatibleWithValidateMethods(attributeClass, member.Type))
                    {
                        var diagnostic = Diagnostic.Create(
                            Rule,
                            attribute.ApplicationSyntaxReference?.GetSyntax(context.CancellationToken).GetLocation(),
                            attributeClass.Name);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private bool IsValidationAttribute(INamedTypeSymbol attributeClass)
        {
            return attributeClass != null &&
                   attributeClass.BaseType != null &&
                   attributeClass.BaseType.ToDisplayString() == "EasyValidate.Abstraction.Attributes.ValidationAttributeBase";
        }

        private bool IsCompatibleWithValidateMethods(INamedTypeSymbol attributeClass, ITypeSymbol memberType)
        {
            var validateMethods = attributeClass
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.Name == "Validate" && m.Parameters.Length == 2) // Ensure Validate has exactly two parameters
                .ToList();

            if (!validateMethods.Any())
                return true; // No specific Validate methods, assume valid usage

            foreach (var method in validateMethods)
            {
                var parameterType = method.Parameters[1].Type;
                if (SymbolEqualityComparer.Default.Equals(memberType, parameterType) || IsAssignableTo(memberType, parameterType))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAssignableTo(ITypeSymbol fromType, ITypeSymbol toType)
        {
            if (SymbolEqualityComparer.Default.Equals(fromType, toType))
                return true;

            if (toType.TypeKind == TypeKind.Interface)
            {
                return fromType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, toType));
            }

            var baseType = fromType.BaseType;
            while (baseType != null)
            {
                if (SymbolEqualityComparer.Default.Equals(baseType, toType))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}
