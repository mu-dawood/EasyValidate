using Microsoft.CodeAnalysis;
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
        public void ProcessPropertyValidation(StringBuilder sb, MemberInfo member)
        {
            DebuggerUtil.Log($"Processing member: {member.Name}");
            DebuggerUtil.Log($"Member type: {member.Type.ToDisplayString()}");

            // Process validation attributes using the new CreateChain pattern
            if (member.Attributes.Any())
            {
                ProcessValidationAttributesWithChain(sb, member);
            }
            if (member.RequireNestedValidation)
                sb.AppendLine($"            if ({member.Name} != null) result.MergeWith(nameof({member.Name}), {member.Name});");

        }


        /// <summary>
        /// Processes validation attributes for a member using the new CreateChain pattern.
        /// </summary>
        private void ProcessValidationAttributesWithChain(StringBuilder sb, MemberInfo member)
        {
            string indent = "            ";

            // Group attributes by Chain property
            var groupedAttributes = member.Attributes.GroupBy(GetChainValue);
            foreach (var chainGroup in groupedAttributes)
            {
                var chainName = chainGroup.Key;
                var attributes = chainGroup.ToList();

                if (attributes.Count > 0)
                {
                    sb.AppendLine($"{indent}// Chain: {chainName}");

                    // Create chain using result.CreateChain(nameof(PropertyName), "chainName")
                    var passedChainValue = chainName switch
                    {
                        null => "string.Empty",
                        "" => "string.Empty",
                        _ => $"\"{chainName}\""
                    };
                    // Generate validation chain with flow control
                    string currentInputVariable = member.Name;
                    var attributeList = attributes.ToList();
                    var currentType = member.Type;
                    for (int i = 0; i < attributeList.Count; i++)
                    {
                        var attr = attributeList[i].Attribute;
                        var (canAccept, resolvedType) = attributeList[i].CanAccept(_compilation, currentType);
                        if (!canAccept)
                        {
                            sb.AppendLine($"{indent}// Proplem with chain for {attr.AttributeClass?.Name} for {currentInputVariable} as it cannot accept type {currentType.ToDisplayString()}");
                            sb.AppendLine($"{indent}throw new InvalidOperationException($\"Attribute {attr.AttributeClass?.Name} cannot accept type {currentType.ToDisplayString()} for {currentInputVariable}.\");");
                            continue;
                        }

                        // Generate variable name for output (based on attribute type)
                        var attributeName = GetAttributeVariableName(attr);
                        var outputVariable = $"{chainName}_{attributeName}_Output".ToLower();

                        // Generate the AddValidtor call with flow control
                        if (attr.AttributeClass.IsNotNullAttribute())
                            sb.AppendLine($"{indent}if (!result.AddValidtor({attributeList[i].InstanceName}, {passedChainValue}, nameof({member.Name}), {currentInputVariable}, out var {outputVariable}))");
                        else if (attr.AttributeClass.IsGeneralAttribute())
                            sb.AppendLine($"{indent}if (!result.AddValidtor<{currentType.GetFullName()}>({attributeList[i].InstanceName}, {passedChainValue}, nameof({member.Name}), {currentInputVariable}, out var {outputVariable}))");
                        else
                            sb.AppendLine($"{indent}if (!result.AddValidtor<{resolvedType!.InputType.GetFullName()}, {resolvedType!.ResolveOutPutType().GetFullName()}>({attributeList[i].InstanceName}, {passedChainValue}, nameof({member.Name}), {currentInputVariable}, out var {outputVariable}))");
                        sb.AppendLine($"{indent}    return;");

                        // Update input variable for next iteration
                        currentInputVariable = outputVariable;
                        currentType = resolvedType!.ResolveOutPutType();
                    }
                }
            }
        }
        //  private void ProcessValidationAttributesWithChain(StringBuilder sb, MemberInfo member)
        //         {
        //             string indent = "            ";

        //             // Group attributes by Chain property
        //             var groupedAttributes = member.Attributes.GroupBy(GetChainValue);
        //             foreach (var chainGroup in groupedAttributes)
        //             {
        //                 var chainName = chainGroup.Key;
        //                 var attributes = chainGroup.ToList();

        //                 if (attributes.Count > 0)
        //                 {
        //                     sb.AppendLine($"{indent}// Chain: {chainName}");

        //                     // Create chain using result.CreateChain(nameof(PropertyName), "chainName")
        //                     var chainVariable = $"{chainName}Chain".Replace(" ", "_").ToLowerInvariant();
        //                     var passedChainValue = chainName switch
        //                     {
        //                         null => "string.Empty",
        //                         "" => "string.Empty",
        //                         _ => $"\"{chainName}\""
        //                     };
        //                     sb.AppendLine($"{indent}var {chainVariable} = result.CreateChain(nameof({member.Name}), {passedChainValue});");

        //                     // Generate validation chain with flow control
        //                     string currentInputVariable = member.Name;
        //                     var attributeList = attributes.ToList();
        //                     var currentType = member.Type;
        //                     for (int i = 0; i < attributeList.Count; i++)
        //                     {
        //                         var attr = attributeList[i].Attribute;
        //                         var (canAccept, resolvedType) = attributeList[i].CanAccept(_compilation, currentType);
        //                         if (!canAccept)
        //                         {
        //                             sb.AppendLine($"{indent}// Proplem with chain for {attr.AttributeClass?.Name} for {currentInputVariable} as it cannot accept type {currentType.ToDisplayString()}");
        //                             sb.AppendLine($"{indent}throw new InvalidOperationException($\"Attribute {attr.AttributeClass?.Name} cannot accept type {currentType.ToDisplayString()} for {currentInputVariable}.\");");
        //                             continue;
        //                         }

        //                         // Generate variable name for output (based on attribute type)
        //                         var attributeName = GetAttributeVariableName(attr);
        //                         var outputVariable = $"{chainName}_{attributeName}_Output".ToLower();

        //                         // Generate the AddValidtor call with flow control
        //                         if (attr.AttributeClass.IsNotNullAttribute())
        //                             sb.AppendLine($"{indent}if (!{chainVariable}.AddValidtor({attributeList[i].InstanceName}, {currentInputVariable}, out var {outputVariable}))");
        //                         else if (attr.AttributeClass.IsGeneralAttribute())
        //                             sb.AppendLine($"{indent}if (!{chainVariable}.AddValidtor<{currentType.GetFullName()}>({attributeList[i].InstanceName}, {currentInputVariable}, out var {outputVariable}))");
        //                         else
        //                             sb.AppendLine($"{indent}if (!{chainVariable}.AddValidtor<{resolvedType!.InputType.GetFullName()}, {resolvedType!.ResolveOutPutType().GetFullName()}>({attributeList[i].InstanceName}, {currentInputVariable}, out var {outputVariable}))");
        //                         sb.AppendLine($"{indent}    return;");

        //                         // Update input variable for next iteration
        //                         currentInputVariable = outputVariable;
        //                         currentType = resolvedType!.ResolveOutPutType();
        //                     }
        //                 }
        //             }
        //         }

        /// <summary>
        /// Gets the Chain property value from a validation attribute.
        /// </summary>
        private string GetChainValue(AttributeInfo attr)
        {
            var chainProperty = attr.Attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Chain");
            return chainProperty.Value.Value?.ToString() ?? "";
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



