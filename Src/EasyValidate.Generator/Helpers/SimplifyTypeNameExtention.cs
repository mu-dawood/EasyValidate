using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Helpers;

internal static class SimplifyTypeNameExtention
{
    internal static string SimplifiedTypeName(this ITypeSymbol typeSymbol)
    {

        // Use the ToDisplayString method with FullyQualifiedFormat to get the full name
        var fullName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        if (typeSymbol.NullableAnnotation == NullableAnnotation.Annotated && typeSymbol.IsReferenceType)
        {
            return fullName.SimplifyTypeName() + "?";
        }
        // Simplify the type name using the extension method
        return fullName.SimplifyTypeName();
    }

    private static string SimplifyTypeName(this string typeName)
    {
        // Remove global:: prefix
        if (typeName.StartsWith("global::"))
        {
            typeName = typeName.Substring(8);
        }

        // Map common system types to their C# keywords
        return typeName switch
        {
            "System.String" => "string",
            "System.Int32" => "int",
            "System.Int64" => "long",
            "System.Double" => "double",
            "System.Decimal" => "decimal",
            "System.Single" => "float",
            "System.Boolean" => "bool",
            "System.DateTime" => "DateTime",
            "System.Object" => "object",
            "System.Byte" => "byte",
            "System.SByte" => "sbyte",
            "System.Int16" => "short",
            "System.UInt16" => "ushort",
            "System.UInt32" => "uint",
            "System.UInt64" => "ulong",
            "System.Collections.IEnumerable" => "IEnumerable",
            _ => typeName
        };
    }
}
