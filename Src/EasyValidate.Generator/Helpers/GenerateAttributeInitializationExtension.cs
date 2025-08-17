using System.Linq;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Helpers;

internal static class GenerateAttributeInitializationExtension
{
    internal static string GenerateAttributeInitialization(this AttributeData attr, AttributeArgumentHandler argumentHandler)
    {
        var attributeClass = attr.AttributeClass!;
        var constructorArguments = argumentHandler.FormatConstructorArguments(attr);

        // Create attribute instance with constructor arguments - use short class name since we have using directive
        var shortClassName = attributeClass.Name;

        // Handle generic attributes by adding type parameters
        if (attributeClass.IsGenericType && attributeClass.TypeArguments.Length > 0)
        {
            var typeArguments = attributeClass.TypeArguments
                .Select(typeArg => typeArg.SimplifiedTypeName())
                .ToArray();
            shortClassName = $"{shortClassName}<{string.Join(", ", typeArguments)}>";
        }

        var attributeCreation = $"new {shortClassName}({string.Join(", ", constructorArguments)})";

        var properties = attr.AttributeClass?
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(p => p.Type.GetFullName() == "global::System.IServiceProvider" || p.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
            .ToList() ?? [];
        // If there are named arguments (property assignments), use object initialization syntax
        if (attr.NamedArguments.Any() || properties.Any())
        {
            // Build object initializer with named arguments
            var namedArguments = attr.NamedArguments
                .Select(namedArg => $"{namedArg.Key} = {argumentHandler.FormatArgument(namedArg.Value)}")
                .ToList();
            foreach (var prop in properties)
            {
                if (prop.GetAttributes().Any(a => a.AttributeClass.IsValidationContext()))
                    namedArguments.Add($"{prop.Name} = this");
                else
                    namedArguments.Add($"{prop.Name} = config?.ServiceProvider ?? throw new InvalidOperationException(\"ServiceProvider is required for {attributeClass.Name}. \")");
            }
            return $"{attributeCreation} {{ {string.Join(", ", namedArguments)} }}";
        }
        else
        {

            // Check for static Instance property of the same type (public or non-public)
            var instanceMembers = attributeClass.GetMembers("Instance")
                  .Where(p => p.IsStatic && p.DeclaredAccessibility == Accessibility.Public);
            foreach (var instanceProperty in instanceMembers)
            {
                ITypeSymbol? type = instanceProperty switch
                {
                    IPropertySymbol prop => prop.Type,
                    IFieldSymbol field => field.Type,
                    _ => null
                };

                if (type is null)
                    continue;

                if (SymbolEqualityComparer.Default.Equals(type, attributeClass))
                {
                    return $"{attributeClass.Name}.Instance";
                }
                else if (type is INamedTypeSymbol namedType &&
                namedType.Name == "Lazy" &&
                namedType.Arity == 1 &&
                namedType.TypeArguments.Length == 1 &&
                SymbolEqualityComparer.Default.Equals(namedType.TypeArguments[0], attributeClass) &&
                namedType.ContainingNamespace.ToDisplayString() == "System")
                {
                    return $"{attributeClass.Name}.Instance.Value";
                }

            }

            return attributeCreation;
        }
    }



}
