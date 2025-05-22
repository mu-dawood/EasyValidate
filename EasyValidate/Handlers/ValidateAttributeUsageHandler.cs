using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers
{
    public class ValidateAttributeUsageHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            ValidateAttributeUsage(classSymbol, context);
            // Call the next handler in the chain
            base.Handle(classSymbol, context, sb);
        }
        private static void ValidateAttributeUsage(INamedTypeSymbol classSymbol, SourceProductionContext context)
        {
            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                foreach (var attr in member.GetAttributes())
                {
                    var attributeType = attr.AttributeClass;
                    if (attributeType == null)
                        continue;

                    // Dynamically retrieve supported types from Validate method overloads
                    var supportedTypes = attributeType
                        .GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(m => m.Name == "Validate" && m.Parameters.Length > 1)
                        .Select(m => m.Parameters[1].Type as ITypeSymbol) // Explicitly cast to ITypeSymbol
                        .Where(t => t != null) // Filter out null values
                        .Distinct(SymbolEqualityComparer.Default) // Use SymbolEqualityComparer.Default for distinct comparison
                        .ToList();

                    if (!supportedTypes.Any())
                    {
                        // If no Validate method overloads are found, allow the attribute to apply to all types
                        continue;
                    }

                    // Check if the attribute has a generic Validate<T> method
                    bool hasGenericValidate = attributeType
                        .GetMembers()
                        .OfType<IMethodSymbol>()
                        .Any(m => m.Name == "Validate" && m.IsGenericMethod);

                    if (hasGenericValidate)
                    {
                        // Skip validation errors if the attribute has a generic Validate<T> method
                        continue;
                    }

                    bool isCompatible = supportedTypes
                        .OfType<ITypeSymbol>() // Ensure all elements are ITypeSymbol
                        .Any(supportedType =>
                            member.Type.Equals(supportedType, SymbolEqualityComparer.Default) ||
                            IsAssignableTo(member.Type, supportedType));

                    if (!isCompatible)
                    {
                        var diagnostic = Diagnostic.Create(
                            new DiagnosticDescriptor(
                                id: "EV001",
                                title: "Invalid Attribute Usage",
                                messageFormat: $"{attributeType.Name} can only be applied to types : {string.Join(", ", supportedTypes.Select(t => t.Name))}.",
                                category: "Usage",
                                DiagnosticSeverity.Error,
                                isEnabledByDefault: true
                            ),
                            member.Locations.FirstOrDefault()
                        );

                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
        private static bool IsAssignableTo(ITypeSymbol fromType, ITypeSymbol toType)
        {
            if (fromType.Equals(toType, SymbolEqualityComparer.Default))
                return true;

            if (toType.TypeKind == TypeKind.Interface)
            {
                return fromType.AllInterfaces.Any(i => i.Equals(toType, SymbolEqualityComparer.Default));
            }

            var baseType = fromType.BaseType;
            while (baseType != null)
            {
                if (baseType.Equals(toType, SymbolEqualityComparer.Default))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }
    }
}
