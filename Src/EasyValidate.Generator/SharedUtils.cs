using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


namespace EasyValidate.Generator;

internal static class SharedUtils
{
    internal static string GetFullName(this ITypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    internal static bool InheritsFrom(this ITypeSymbol? type, string baseTypeFullName)
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

    // internal static bool IsEasyValidateTarget(this ITypeSymbol? type)=>
    //     type?.InheritsFrom("EasyValidate.Abstractions.EasyValidateAttribute") ?? false;

    internal static ITypeSymbol ToNonNullable(this ITypeSymbol type)
    {
        if (type is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
        {
            return namedType.TypeArguments[0];
        }
        return type.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
    }

    internal static bool IsValidationAttribute(this ITypeSymbol? type, out ImmutableArray<InputAndOutputTypes> args)
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
            if (interfaceType is INamedTypeSymbol namedInterface && namedInterface.TypeArguments.Length >= 1)
            {
                bool isAsync = false;
                if (namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::EasyValidate.Abstractions.IAsyncValidationAttribute"))
                    isAsync = true;
                else if (!namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::EasyValidate.Abstractions.IValidationAttribute"))
                    continue;
                if (namedInterface.TypeArguments.Length == 1)
                {
                    arguments.Add(new(namedInterface.TypeArguments[0], namedInterface.TypeArguments[0], isAsync, type.IsOptionalOrNotNullAttribute()));
                }
                else
                    arguments.Add(new(namedInterface.TypeArguments[0], namedInterface.TypeArguments[1], isAsync, type.IsOptionalOrNotNullAttribute()));
                res = true;
            }
        }
        args = [.. arguments];
        return res;
    }
    internal static bool IsValidationAttributeBase(this ITypeSymbol? type)
    {

        if (type == null)
            return false;
        string[] interfaces = [
            "global::EasyValidate.Abstractions.IValidationAttribute",
            "global::EasyValidate.Abstractions.IAsyncValidationAttribute",
        ];
        // Check all interfaces transitively (including interfaces implemented by base classes)
        foreach (var interfaceType in type.AllInterfaces)
        {
            if (interfaceType is INamedTypeSymbol namedInterface &&
                interfaces.Any((x) => namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith(x)))
            {
                return true;
            }
        }

        return false;
    }
    internal static bool IsValidationAttribute(this ITypeSymbol? type)
    {

        if (type == null)
            return false;
        string[] interfaces = [
            "global::EasyValidate.Abstractions.IValidationAttribute",
            "global::EasyValidate.Abstractions.IAsyncValidationAttribute",
        ];
        // Check all interfaces transitively (including interfaces implemented by base classes)
        foreach (var interfaceType in type.AllInterfaces)
        {
            if (interfaceType is INamedTypeSymbol namedInterface &&
                interfaces.Any((x) => namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith(x)) &&
                namedInterface.TypeArguments.Length >= 1)
            {
                return true;
            }
        }

        return false;
    }



    internal static bool ImplementsInterface(this ITypeSymbol? type, string interfaceFullName)
    {
        if (type == null)
            return false;
        var _interface = interfaceFullName.StartsWith("global::")
        ? interfaceFullName
        : "global::" + interfaceFullName;

        // Check all interfaces transitively (including interfaces implemented by base classes)
        foreach (var interfaceType in type.AllInterfaces)
        {
            if (interfaceType is INamedTypeSymbol namedInterface && namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith(_interface))
            {
                return true;
            }
        }

        return false;
    }

    internal static bool IsCollectionOfInterface(this ITypeSymbol? type, string interfaceFullName)
    {
        if (type == null)
            return false;

        ITypeSymbol? elementType = null;

        if (type is IArrayTypeSymbol arrayType)
        {
            elementType = arrayType.ElementType;
        }
        else if (type is INamedTypeSymbol namedType)
        {
            var ienumerable = namedType.AllInterfaces
                .FirstOrDefault(i => i.Name == "IEnumerable" && i.IsGenericType);

            if (ienumerable?.TypeArguments.Length == 1)
                elementType = ienumerable.TypeArguments[0];
        }

        return elementType != null && elementType.ImplementsInterface(interfaceFullName);
    }


    internal static bool ImplementsIAsyncValidate(this ITypeSymbol? type) =>
        type?.ImplementsInterface("EasyValidate.Abstractions.IAsyncValidate") ?? false;

    internal static bool IsValidationContext(this ITypeSymbol? type) =>
         type?.InheritsFrom("EasyValidate.Abstractions.ValidationContextAttribute") ?? false;
    internal static bool IsCollectionOfIAsyncValidate(this ITypeSymbol? type) =>
        type?.IsCollectionOfInterface("EasyValidate.Abstractions.IAsyncValidate") ?? false;

    internal static bool ImplementsIValidate(this ITypeSymbol? type) =>
        type?.ImplementsInterface("EasyValidate.Abstractions.IValidate") ?? false;

    internal static bool ImplementsIGenerate(this ITypeSymbol? type) => type?.ImplementsInterface("EasyValidate.Abstractions.IGenerate") ?? false;
    internal static bool IsCollectionOfIValidate(this ITypeSymbol? type) =>
        type?.IsCollectionOfInterface("EasyValidate.Abstractions.IValidate") ?? false;


    internal static bool IsNotNullAttribute(this ITypeSymbol? type) => InheritsFrom(type, "EasyValidate.Attributes.NotNullAttribute");
    internal static bool IsOptionalAttribute(this ITypeSymbol? type) => InheritsFrom(type, "EasyValidate.Attributes.OptionalAttribute");
    internal static bool IsOptionalOrNotNullAttribute(this ITypeSymbol? type) => type.IsOptionalAttribute() || type.IsNotNullAttribute();


    internal static T? GetArgumentValue<T>(this ImmutableArray<KeyValuePair<string, TypedConstant>> arguments, string argumentName)
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

    internal static string GetChainValue(this AttributeData attribute)
        => attribute.NamedArguments.GetArgumentValue<string>("Chain") ?? string.Empty;

    internal static (bool areCompatible, InputAndOutputTypes?) CanAccept(this ImmutableArray<InputAndOutputTypes> inputAndOutputs, Compilation compilation, ITypeSymbol type)
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

    internal static (bool isAsync, ImmutableArray<ITypeSymbol> arguments) IsAsyncMethod(this IMethodSymbol type)
    {
        if (type.IsAsync) return (true, type.TypeArguments);

        if (type.ReturnType is INamedTypeSymbol namedTypeSymbol)
        {
            // Check for GetAwaiter() method
            var getAwaiterMethod = namedTypeSymbol.GetMembers("GetAwaiter").OfType<IMethodSymbol>().FirstOrDefault(m => m.Parameters.Length == 0);
            if (getAwaiterMethod != null)
            {
                if (getAwaiterMethod.ReturnType is INamedTypeSymbol awaiterType)
                {
                    // Awaiter must have IsCompleted property and GetResult() method
                    var hasIsCompleted = awaiterType.GetMembers("IsCompleted").OfType<IPropertySymbol>().Any();
                    var getResultMethod = awaiterType.GetMembers("GetResult").OfType<IMethodSymbol>().FirstOrDefault(m => m.Parameters.Length == 0);
                    if (hasIsCompleted && getResultMethod != null)
                    {
                        return (true, namedTypeSymbol.TypeArguments);
                    }
                }
            }

        }
        return (false, ImmutableArray<ITypeSymbol>.Empty);
    }
    private static readonly HashSet<string> CSharpKeywords = new()
    {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
        "char", "checked", "class", "const", "continue", "decimal", "default",
        "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
        "false", "finally", "fixed", "float", "for", "foreach", "goto", "if",
        "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
        "namespace", "new", "null", "object", "operator", "out", "override", "params",
        "private", "protected", "public", "readonly", "ref", "return", "sbyte",
        "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct",
        "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
        "unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
    };
    internal static string ToCSharpVariableName(this string input)
    {
        var name = input._ToCSharpVariableName();
        return CSharpKeywords.Contains(name) ? "@" + name : name;
    }

    private static string _ToCSharpVariableName(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "_";

        // Split the input on non-alphanumeric characters
        var parts = Regex.Split(input, @"[\W_]+")
                         .Where(p => !string.IsNullOrWhiteSpace(p))
                         .ToArray();
        if (parts.Length == 0)
            return "_";
        // Capitalize first letter of each part, preserve rest (PascalCase)
        var pascal = string.Concat(parts.Select(CapitalizeFirst));
        // Lowercase the first character to make camelCase
        var camel = char.ToLowerInvariant(pascal[0]) + pascal.Substring(1);

        // Ensure valid C# identifier (can't start with digit)
        if (char.IsDigit(camel[0]))
            camel = "_" + camel;

        return camel;
    }



    internal static string ToPascalCase(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var parts = Regex.Split(name, @"[\W_]+")
                        .Where(p => !string.IsNullOrEmpty(p));
        // Capitalize only the first letter, preserve the rest
        return string.Concat(parts.Select(CapitalizeFirst));
    }
    private static string CapitalizeFirst(string part)
    {
        if (string.IsNullOrEmpty(part)) return part;
        return char.ToUpperInvariant(part[0]) + part.Substring(1);
    }

}





internal class InputAndOutputTypes(ITypeSymbol inputType, ITypeSymbol outputType, bool isAsync, bool isOptionalOrNotNullAttribute)
{
    public bool IsAsync { get; } = isAsync;
    public ITypeSymbol InputType { get; } = inputType;
    private readonly ITypeSymbol OutputType = outputType;
    public bool IsOptionalOrNotNullAttribute { get; set; } = isOptionalOrNotNullAttribute;
    public bool RequireTransformation => !SymbolEqualityComparer.Default.Equals(InputType, OutputType);

    public ITypeSymbol ResolveOutPutType(ITypeSymbol? previousType)
    {
        if (!IsOptionalOrNotNullAttribute || previousType == null) return OutputType;
        // For value types, strip nullable
        if (previousType is INamedTypeSymbol namedType &&
            namedType.IsGenericType &&
            namedType.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
        {
            return namedType.TypeArguments[0];
        }
        // For reference types, remove nullable annotation
        return previousType.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
    }

}