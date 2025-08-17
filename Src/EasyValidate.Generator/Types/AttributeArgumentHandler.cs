using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Types
{
    /// <summary>
    /// Handles formatting of attribute constructor and named arguments for code generation.
    /// </summary>
    internal class AttributeArgumentHandler
    {
        private static readonly Dictionary<Type, Func<object, string>> PrimitiveFormatters = new()
        {
            { typeof(bool), value => (bool)value ? "true" : "false" },
            { typeof(string), value => $"\"{value.ToString().Replace("\\", "\\\\")}\"" },
            { typeof(char), value => $"\'{value}\'" },
            { typeof(int), value => value.ToString() },
            { typeof(double), value => value.ToString() },
            { typeof(float), value => value.ToString() },
            { typeof(long), value => value.ToString() },
            { typeof(short), value => value.ToString() },
            { typeof(byte), value => value.ToString() },
            { typeof(decimal), value => value.ToString() },
            { typeof(uint), value => value.ToString() },
            { typeof(ulong), value => value.ToString() },
            { typeof(ushort), value => value.ToString() },
            { typeof(sbyte), value => value.ToString() }
        };

        /// <summary>
        /// Formats constructor arguments for an attribute.
        /// </summary>
        internal List<string> FormatConstructorArguments(AttributeData attribute)
        {
            var constructorArguments = new List<string>();
            foreach (var arg in attribute.ConstructorArguments)
            {
                constructorArguments.Add(FormatArgument(arg));
            }
            return constructorArguments;
        }

        /// <summary>
        /// Formats a single typed constant argument.
        /// </summary>
        internal string FormatArgument(TypedConstant arg)
        {
            return arg.Kind switch
            {
                TypedConstantKind.Primitive => arg.Value != null && PrimitiveFormatters.TryGetValue(arg.Value.GetType(), out var formatter)
                    ? formatter(arg.Value)
                    : arg.Value?.ToString() ?? "null",
                TypedConstantKind.Enum => $"({arg.Type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}){arg.Value}",
                TypedConstantKind.Type => $"typeof({arg.Value})",
                TypedConstantKind.Array => FormatArrayArgument(arg),
                TypedConstantKind.Error => "null", // Handle error case gracefully
                _ => "null"
            };
        }

        /// <summary>
        /// Formats array arguments.
        /// </summary>
        private string FormatArrayArgument(TypedConstant arg)
        {
            var arrayValues = arg.Values.Select(FormatArgument).ToList();
            return $"new[] {{ {string.Join(", ", arrayValues)} }}";
        }
    }
}
