using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate;

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
    public static bool IsGeneralAttribute(this ITypeSymbol? type) => InheritsFrom(type, "EasyValidate.Core.Attributes.GeneralValidationAttributeBase");
    public static string? GetFullName(this ITypeSymbol? type) => type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    public static string ToSakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (char.IsUpper(c))
            {
                if (i > 0)
                    sb.Append('_');

                sb.Append(char.ToLowerInvariant(c));
            }else if (c == ' ')
            {
                if (i > 0 && sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
            }
            else if (c == '-')
            {
                if (i > 0 && sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
            }
            else if (c == '_')
            {
                if (sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
            }
            else if (c == '@')
            {
                if (i > 0 && sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
                sb.Append("at_");
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static string ToPascalCase(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var words = name.Split(['_', '@'], StringSplitOptions.RemoveEmptyEntries);
        var result = new StringBuilder(name.Length);

        foreach (var word in words)
        {
            if (word.Length == 0) continue;

            result.Append(char.ToUpperInvariant(word[0]));
            if (word.Length > 1)
                result.Append(word.Substring(1).ToLowerInvariant());
        }

        return result.ToString();
    }

    public static (bool areCompatible, InputAndOutputTypes?) CanAccept(this AttributeInfo attribute, Compilation compilation, ITypeSymbol type)
    {
        if (attribute.Attribute.AttributeClass.IsNotNullAttribute()) return (true, new(type, type, true));
        foreach (var info in attribute.InputAndOutputTypes)
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