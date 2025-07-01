using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;


namespace EasyValidate.Analyzers;

public static class Utils
{
    public static bool InheritsFrom(this ITypeSymbol? type, string baseTypeFullName)
    {
        var baseGlobalTypeFullName = baseTypeFullName.StartsWith("global::")
            ? baseTypeFullName
            : "global::" + baseTypeFullName;
        while (type != null)
        {
            if (type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == baseGlobalTypeFullName) return true;
            type = type.BaseType;
        }
        return false;
    }
    public static bool IsValidationAttribute(this ITypeSymbol? type, out ImmutableArray<InputAndOutputTypes> args)
    {

        if (type == null)
        {
            args = [];
            return false;
        }
        var arguments = new List<InputAndOutputTypes>();
        var res = false;
        // Check all interfaces transitively (including interfaces implemented by base classes)
        foreach (var interfaceType in type.AllInterfaces)
        {
            if (interfaceType is INamedTypeSymbol namedInterface &&
                namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    .StartsWith("global::EasyValidate.Core.Abstraction.IValidationAttribute") &&
                namedInterface.TypeArguments.Length == 2)
            {
                arguments.Add(new(namedInterface.TypeArguments[0], namedInterface.TypeArguments[1], type.IsNotNullAttribute()));
                res = true;
            }
        }
        args = [.. arguments];
        return res;
    }

    public static bool IsValidationAttribute(this ITypeSymbol? type)
    {

        if (type == null)
            return false;

        // Check all interfaces transitively (including interfaces implemented by base classes)
        foreach (var interfaceType in type.AllInterfaces)
        {
            if (interfaceType is INamedTypeSymbol namedInterface &&
                namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                    .StartsWith("global::EasyValidate.Core.Abstraction.IValidationAttribute") &&
                namedInterface.TypeArguments.Length >= 2)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsNotNullAttribute(this ITypeSymbol? type) => InheritsFrom(type, "EasyValidate.Core.Attributes.NotNullAttribute");

    public static ITypeSymbol MakeNullable(this SymbolAnalysisContext context, ITypeSymbol type)
    {
        if (type.NullableAnnotation == NullableAnnotation.Annotated)
            return type;

        if (type.IsReferenceType)
            return type.WithNullableAnnotation(NullableAnnotation.Annotated);

        if (type is INamedTypeSymbol named && named.OriginalDefinition.SpecialType != SpecialType.System_Nullable_T)
        {
            var nullableType = context.Compilation.GetTypeByMetadataName("System.Nullable`1");
            if (nullableType != null)
            {
                return nullableType.Construct(type);
            }
        }

        return type;
    }


    public static T? GetArgumentValue<T>(this ImmutableArray<KeyValuePair<string, TypedConstant>> arguments, string argumentName)
    {
        if (arguments.IsDefaultOrEmpty)
            return default;

        foreach (var namedArg in arguments)
        {
            if (namedArg.Key != null && namedArg.Key == argumentName && namedArg.Value.Value is T value)
            {
                return value;
            }
        }
        return default;
    }

    public static string GetChainValue(this AttributeData attribute)
        => attribute.NamedArguments.GetArgumentValue<string>("Chain") ?? string.Empty;



    public static (bool areCompatible, InputAndOutputTypes?) CanAccept(this ImmutableArray<InputAndOutputTypes> inputAndOutputs, Compilation compilation, ITypeSymbol type)
    {
        foreach (var info in inputAndOutputs)
        {
            if (AreTypesCompatible(compilation, info.InputType, type))
            {
                return (true, info);
            }
        }
        return (false, null);
    }
    private static bool AreTypesCompatible(Compilation compilation, ITypeSymbol inputType, ITypeSymbol passedType)
    {
        // Check if T matches the property type or is compatible
        if (SymbolEqualityComparer.Default.Equals(inputType, passedType))
        {
            // Also check nullability annotation for reference types
            if (inputType.IsReferenceType && passedType.IsReferenceType)
            {
                if (inputType.NullableAnnotation != passedType.NullableAnnotation)
                {
                    return false;
                }
            }
            return true;
        }
        if (compilation.ClassifyConversion(passedType, inputType).IsImplicit)
        {
            // For reference types, check nullability compatibility
            if (inputType.IsReferenceType && passedType.IsReferenceType)
            {
                if (inputType.NullableAnnotation != passedType.NullableAnnotation)
                {
                    return false;
                }
            }
            return true;
        }
        return false;

    }

}





public class InputAndOutputTypes(ITypeSymbol inputType, ITypeSymbol outputType, bool isNotNullAttribute)
{
    public ITypeSymbol InputType { get; set; } = inputType;
    public ITypeSymbol OutputType { private get; set; } = outputType;
    public bool IsNotNullAttribute { get; set; } = isNotNullAttribute;
    public ITypeSymbol ResolveOutPutType()
    {
        if (!IsNotNullAttribute) return OutputType;
        // For value types, strip nullable
        if (InputType is INamedTypeSymbol namedType &&
            namedType.IsGenericType &&
            namedType.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
        {
            return namedType.TypeArguments[0];
        }
        // For reference types, remove nullable annotation
        return InputType.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
    }

}