
/// this file is shared between 2 liberaries so its important to keep these usings even they are not used
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EasyValidate;

public static class SharedUtils
{
    public static string GetFullName(this ITypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

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

    public static ITypeSymbol ToNonNullable(this ITypeSymbol type)
    {
        if (type is INamedTypeSymbol namedType && namedType.IsGenericType && namedType.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
        {
            return namedType.TypeArguments[0];
        }
        return type.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
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
            if (interfaceType is INamedTypeSymbol namedInterface && namedInterface.TypeArguments.Length >= 1)
            {
                bool isAsync = false;
                if (namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::EasyValidate.Core.Abstraction.IAsyncValidationAttribute"))
                    isAsync = true;
                else if (!namedInterface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat).StartsWith("global::EasyValidate.Core.Abstraction.IValidationAttribute"))
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

    public static bool IsValidationAttribute(this ITypeSymbol? type)
    {

        if (type == null)
            return false;
        string[] interfaces = [
            "global::EasyValidate.Core.Abstraction.IValidationAttribute",
            "global::EasyValidate.Core.Abstraction.IAsyncValidationAttribute",
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



    public static bool ImplementsInterface(this ITypeSymbol? type, string interfaceFullName)
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

    public static bool IsCollectionOfInterface(this ITypeSymbol? type, string interfaceFullName)
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


    public static bool ImplementsIAsyncValidate(this ITypeSymbol? type) =>
        type?.ImplementsInterface("EasyValidate.Core.Abstraction.IAsyncValidate") ?? false;

    public static bool IsValidationContext(this ITypeSymbol? type) =>
         type?.InheritsFrom("EasyValidate.Core.Attributes.ValidationContextAttribute") ?? false;
    public static bool IsCollectionOfIAsyncValidate(this ITypeSymbol? type) =>
        type?.IsCollectionOfInterface("EasyValidate.Core.Abstraction.IAsyncValidate") ?? false;

    public static bool ImplementsIValidate(this ITypeSymbol? type) =>
        type?.ImplementsInterface("EasyValidate.Core.Abstraction.IValidate") ?? false;
    public static bool IsCollectionOfIValidate(this ITypeSymbol? type) =>
        type?.IsCollectionOfInterface("EasyValidate.Core.Abstraction.IValidate") ?? false;


    public static bool IsNotNullAttribute(this ITypeSymbol? type) => InheritsFrom(type, "EasyValidate.Core.Attributes.NotNullAttribute");
    public static bool IsOptionalAttribute(this ITypeSymbol? type) => InheritsFrom(type, "EasyValidate.Core.Attributes.OptionalAttribute");
    public static bool IsOptionalOrNotNullAttribute(this ITypeSymbol? type) => type.IsOptionalAttribute() || type.IsNotNullAttribute();


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

    public static (bool isAsync, ImmutableArray<ITypeSymbol> arguments) IsAsyncMethod(this IMethodSymbol type)
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

    public static string ToCSharpVariableName(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "_";

        // Convert to camelCase and remove invalid characters
        var sb = new StringBuilder();
        bool nextUpper = false;

        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c))
            {
                if (sb.Length == 0)
                {
                    sb.Append(char.ToLowerInvariant(c));
                }
                else if (nextUpper)
                {
                    sb.Append(char.ToUpperInvariant(c));
                    nextUpper = false;
                }
                else
                {
                    sb.Append(c);
                }
            }
            else if (c is ' ' or '_' or '-' or '@')
            {
                nextUpper = true;
            }
            // else ignore other special characters entirely
        }

        if (sb.Length == 0)
            return "_";

        var result = sb.ToString();

        // If it starts with a digit, prefix with "_"
        if (char.IsDigit(result[0]))
            result = "_" + result;

        // If it's a reserved keyword, prefix with "@"
        if (CSharpKeywords.Contains(result))
            result = "@" + result;

        return result;
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

}





public class InputAndOutputTypes(ITypeSymbol inputType, ITypeSymbol outputType, bool isAsync, bool isOptionalOrNotNullAttribute)
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