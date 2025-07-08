using Microsoft.CodeAnalysis;
using System.Collections.Generic;
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
        public void ProcessPropertyValidation(StringBuilder sb, ITypeSymbol type, string memberName, string chainName, List<AttributeInfo> attributes)
        {
            DebuggerUtil.Log($"Processing member: {memberName} with chain: {chainName}");
            string indent = "            ";

            if (attributes.Count > 0)
            {
                // Generate validation chain with flow control
                string currentInputVariable = memberName;
                var currentType = type;
                for (int i = 0; i < attributes.Count; i++)
                {
                    var attr = attributes[i].Attribute;
                    var (canAccept, resolvedType) = attributes[i].CanAccept(_compilation, currentType);
                    if (!canAccept)
                    {
                        sb.AppendLine($"{indent}// Proplem with chain for {attr.AttributeClass?.Name} for {currentInputVariable} as it cannot accept type {currentType.ToDisplayString()}");
                        sb.AppendLine($"{indent}throw new InvalidOperationException($\"Attribute {attr.AttributeClass?.Name} cannot accept type {currentType.ToDisplayString()} for {currentInputVariable}.\");");
                        continue;
                    }

                    // Generate variable name for output (based on attribute type)
                    var attributeName = GetAttributeVariableName(attr);
                    var outputVariable = $"{attributeName}Output".ToSakeCase();

                    // Generate the AddValidtor call with flow control
                    if (attr.AttributeClass.IsOptionalOrNotNullAttribute())
                    {
                        sb.AppendLine($"{indent}if (!chain.AddValidtor<{resolvedType!.ResolveOutPutType().GetFullName()}>({attributes[i].InstanceName}, {currentInputVariable}, out {resolvedType!.ResolveOutPutType().GetFullName()} {outputVariable}))");
                        currentInputVariable = outputVariable;
                    }
                    else if (attr.AttributeClass.IsOptionalOrNotNullAttribute())
                    {
                        sb.AppendLine($"{indent}if (!chain.AddValidtor<{resolvedType!.ResolveOutPutType().GetFullName()}>({attributes[i].InstanceName}, {currentInputVariable}, out {resolvedType!.ResolveOutPutType().GetFullName()} {outputVariable}))");
                        currentInputVariable = outputVariable;
                    }
                    else if (resolvedType!.RequireTransformation)
                    {
                        sb.AppendLine($"{indent}if (!chain.AddValidtor<{resolvedType!.InputType.GetFullName()}, {resolvedType!.ResolveOutPutType().GetFullName()}>({attributes[i].InstanceName}, {currentInputVariable}, out {resolvedType!.ResolveOutPutType().GetFullName()} {outputVariable}))");
                        currentInputVariable = outputVariable;
                    }
                    else
                        sb.AppendLine($"{indent}if (!chain.AddValidtor({attributes[i].InstanceName}, {currentInputVariable}))");
                    sb.AppendLine($"{indent}    return;");

                    // Update input variable for next iteration
                    currentType = resolvedType!.ResolveOutPutType();
                }
            }

        }

        public void ProcessNestedValidation(StringBuilder sb, MemberInfo member)
        {
            // If the member requires nested validation, generate the call
            if (member.RequireNestedValidation)
            {
                sb.AppendLine($"            if ({member.Name} != null) result.MergeWith(nameof({member.Name}), {member.Name});");
            }
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



