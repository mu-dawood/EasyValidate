using EasyValidate.Types;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public bool ProcessPropertyValidation(StringBuilder sb, MemberInfo member, string chainVariableName, string returnVariable, List<AttributeInfo> attributes)
        {
            string indent = "            ";
            bool awaitable = false;
            if (attributes.Count > 0)
            {
                // Generate validation with flow control
                string currentInputVariable = member.Name;
                var currentType = member.Type;
                for (int i = 0; i < attributes.Count; i++)
                {
                    var info = attributes[i];
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

                    if (resolvedType!.IsOptionalOrNotNullAttribute)
                    {
                        var awaitKeyword = resolvedType.IsAsync || info.ConditionalMethod?.IsAsync == true ? "await " : string.Empty;
                        if (awaitKeyword == "await ") awaitable = true;
                        if (info.ConditionalMethod != null)
                            sb.AppendLine($"{indent}if ({awaitKeyword}{attributes[i].InstanceName}.IsValid({chainVariableName}, {currentInputVariable}, {info.ConditionalMethod.MethodName}))");
                        else
                            sb.AppendLine($"{indent}if ({awaitKeyword}{attributes[i].InstanceName}.IsValid({chainVariableName}, {currentInputVariable}))");
                        sb.AppendLine($"{indent}    return {returnVariable};");
                        var outputVariable = $"{attributeName}Output".ToSakeCase();
                        /// check is currentType conatians 'Value' property like DateTime, Guid, etc.
                        if (currentType is INamedTypeSymbol namedType && namedType.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == "HasValue" && p.Type.SpecialType == SpecialType.System_Boolean))
                            sb.AppendLine($"{indent}var {outputVariable} = {currentInputVariable}.HasValue ? {currentInputVariable}.Value : default!;");
                        else
                            sb.AppendLine($"{indent}var {outputVariable} = {currentInputVariable}!;");
                        currentInputVariable = outputVariable;

                    }
                    else if (resolvedType!.RequireTransformation)
                    {
                        var outputVariable = $"{attributeName}Output".ToSakeCase();
                        var isValidVariable = $"{attributeName}IsValid".ToSakeCase();

                        var awaitKeyword = resolvedType.IsAsync || info.ConditionalMethod?.IsAsync == true ? "await " : string.Empty;
                        if (awaitKeyword == "await ") awaitable = true;
                        if (info.ConditionalMethod != null)
                            sb.AppendLine($"{indent}var ({isValidVariable}, {outputVariable}) = {awaitKeyword}{attributes[i].InstanceName}.IsValid({chainVariableName}, {currentInputVariable}, {info.ConditionalMethod.MethodName});");
                        else
                            sb.AppendLine($"{indent}var ({isValidVariable}, {outputVariable}) = {awaitKeyword}{attributes[i].InstanceName}.IsValid({chainVariableName}, {currentInputVariable});");
                        sb.AppendLine($"{indent}if(!{isValidVariable})");
                        sb.AppendLine($"{indent}    return {returnVariable};");
                        currentInputVariable = outputVariable;
                    }
                    else
                    {
                        var awaitKeyword = resolvedType.IsAsync || info.ConditionalMethod?.IsAsync == true ? "await " : string.Empty;
                        if (awaitKeyword == "await ") awaitable = true;
                        if (info.ConditionalMethod != null)
                            sb.AppendLine($"{indent}if ({awaitKeyword}{attributes[i].InstanceName}.IsValid({chainVariableName}, {currentInputVariable}, {info.ConditionalMethod.MethodName}))");
                        else
                            sb.AppendLine($"{indent}if ({awaitKeyword}{attributes[i].InstanceName}.IsValid({chainVariableName}, {currentInputVariable}))");
                        sb.AppendLine($"{indent}    return {returnVariable};");
                    }

                    // Update input variable for next iteration
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



