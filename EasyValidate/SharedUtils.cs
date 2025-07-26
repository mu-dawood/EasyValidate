using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

    public static (bool, ImmutableArray<ITypeSymbol> arguments) IsAsyncType(this ITypeSymbol type)
    {
        if (type is INamedTypeSymbol namedTypeSymbol)
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

}





public class InputAndOutputTypes(ITypeSymbol inputType, ITypeSymbol outputType, bool isAsync, bool isOptionalOrNotNullAttribute)
{
    public bool IsAsync { get; } = isAsync;
    public Microsoft.CodeAnalysis.ITypeSymbol InputType { get; } = inputType;
    private readonly Microsoft.CodeAnalysis.ITypeSymbol OutputType = outputType;
    public bool IsOptionalOrNotNullAttribute { get; set; } = isOptionalOrNotNullAttribute;
    public bool RequireTransformation => !Microsoft.CodeAnalysis.SymbolEqualityComparer.Default.Equals(InputType, OutputType);

    public Microsoft.CodeAnalysis.ITypeSymbol ResolveOutPutType()
    {
        if (!IsOptionalOrNotNullAttribute) return OutputType;
        // For value types, strip nullable
        if (InputType is Microsoft.CodeAnalysis.INamedTypeSymbol namedType &&
            namedType.IsGenericType &&
            namedType.ConstructedFrom.SpecialType == Microsoft.CodeAnalysis.SpecialType.System_Nullable_T)
        {
            return namedType.TypeArguments[0];
        }
        // For reference types, remove nullable annotation
        return InputType.WithNullableAnnotation(Microsoft.CodeAnalysis.NullableAnnotation.NotAnnotated);
    }

}