using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Handlers
{
    /// <summary>
    /// Coordinates validation processing for properties by delegating to specialized handlers.
    /// </summary>
    internal class ValidationAttributeProcessorHandler(Compilation compilation)
    {
        private readonly Compilation _compilation = compilation;



        /// <summary>
        /// Processes all validation for a property including attributes and nested validation.
        /// </summary>
        public bool ProcessPropertyValidation(StringBuilder sb, Member member, IReadOnlyCollection<AttributeInfo> attributes)
        {
            string indent = "            ";
            var awaitable = false;

            if (attributes.Count > 0)
            {
                // Generate validation with flow control
                string currentInputVariable = member.Name;
                var currentType = member.Type;
                for (int i = 0; i < attributes.Count; i++)
                {
                    var info = attributes.ElementAt(i);
                    var attr = info.Attribute;
                    var (canAccept, resolvedType) = attr.AttributeClass.IsOptionalOrNotNullAttribute() ?
                        (true, new(currentType, currentType, false, true)) : info.InputAndOutputTypes.CanAccept(_compilation, currentType);
                    if (!canAccept)
                    {
                        sb.AppendLine($"{indent}// Problem with {attr.AttributeClass?.Name} for {currentInputVariable} as it cannot accept type {currentType.ToDisplayString()}");
                        sb.AppendLine($"{indent}throw new InvalidOperationException($\"Attribute {attr.AttributeClass?.Name} cannot accept type {currentType.ToDisplayString()} for {currentInputVariable}.\");");
                        continue;
                    }

                    // Generate variable name for output (based on attribute type)
                    var attributeName = GetAttributeVariableName(attr);
                    var validationResultVaiable = $"{attributeName}Result".ToCSharpVariableName();
                    var attrInstance = info.InstanceVariable;
                    if (info.ConditionalMethod != null)
                    {
                        var conditionalAwait = info.ConditionalMethod.IsAsync ? "await " : "";
                        sb.AppendLine($"{indent}if ({conditionalAwait}{info.ConditionalMethod.MethodName}(result)) {{");
                        indent += "    ";
                    }
                    var resultAawit = resolvedType!.IsAsync ? "await " : "";
                    if (attr.AttributeClass.IsOptionalAttribute())
                    {
                        sb.AppendLine($"{indent}if ({currentInputVariable} is null) return result;");
                        // check if currentInputVariable is nullable and if so, remove the nullability
                        // if contains 'Value' property like DateTime, Guid, etc.
                        if (currentType is INamedTypeSymbol namedType && namedType.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == "HasValue" && p.Type.SpecialType == SpecialType.System_Boolean))
                            currentInputVariable = $"{currentInputVariable}.Value";
                    }
                    else
                    {
                        var config = info.NeedServiceProvider() ?
                            "config" : string.Empty;
                        var validateMethodName = resolvedType!.IsAsync ? "ValidateAsync" : "Validate";
                        sb.AppendLine($"{indent}var {attrInstance} = {info.InstanceMethod}({config});");
                        sb.AppendLine($"{indent}var {validationResultVaiable} = {resultAawit}{attrInstance}.{validateMethodName}(nameof({member.Name}), {currentInputVariable});");
                        sb.AppendLine($"{indent}if(!{validationResultVaiable}.IsValid) {{");
                        sb.AppendLine($"{indent}    result.AddResult({validationResultVaiable},{attrInstance},{currentInputVariable});");
                        sb.AppendLine($"{indent}    return result;");
                        sb.AppendLine($"{indent}}}");
                        if (attr.AttributeClass.IsNotNullAttribute())
                        {
                            // check if currentInputVariable is nullable and if so, remove the nullability
                            // if contains 'Value' property like DateTime, Guid, etc.
                            if (currentType is INamedTypeSymbol namedType && namedType.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == "HasValue" && p.Type.SpecialType == SpecialType.System_Boolean))
                                currentInputVariable = $"{currentInputVariable}.Value";
                            else
                                currentInputVariable = $"{currentInputVariable}!";
                        }
                        else if (resolvedType!.RequireTransformation)
                            currentInputVariable = $"{validationResultVaiable}.Output";
                    }
                    if (info.ConditionalMethod != null)
                    {
                        sb.AppendLine($"{indent}}}");
                    }
                    awaitable = resolvedType!.IsAsync || info.ConditionalMethod?.IsAsync == true;
                    currentType = resolvedType!.ResolveOutPutType(currentType);
                }
            }
            return awaitable;
        }


        /// <summary>
        /// Gets a variable name based on the attribute type for generating output variables.
        /// </summary>
        private string GetAttributeVariableName(AttributeData attr)
        {
            var attributeClass = attr.AttributeClass!;
            var className = attributeClass.Name;

            // Remove "Attribute" suffix if present
            if (className.EndsWith("Attribute"))
            {
                className = className.Substring(0, className.Length - 9);
            }

            // Convert to camelCase
            return char.ToLowerInvariant(className[0]) + className.Substring(1);
        }
    }
}



