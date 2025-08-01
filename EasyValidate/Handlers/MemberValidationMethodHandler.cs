using EasyValidate.Types;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace EasyValidate.Handlers
{
    /// <summary>
    /// Generates individual private validation methods for each property.
    /// </summary>
    internal class MemberValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            List<string> awaitableMembersList = [];
            // Process members
            foreach (var member in @params.Target.Members)
            {
                if (GeneratePropertyValidationMethod(sb, member))
                {
                    awaitableMembersList.Add(member.Name);
                }
            }

            if (awaitableMembers.TryGetValue(@params.Target.Symbol.Name, out var existingList))
            {
                awaitableMembersList.AddRange(existingList);
            }
            awaitableMembers[@params.Target.Symbol.Name] = [.. awaitableMembersList.Distinct()];
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private bool GeneratePropertyValidationMethod(StringBuilder sb, MemberInfo member)
        {

            var groupedAttributes = member.Attributes.GroupBy(GetChainValue).OrderBy(g => string.IsNullOrEmpty(g.Key) ? 1 : 0).ThenBy(g => g.Key).ToList();

            var methodName = $"Validate@{member.Name}".ToPascalCase();
            StringBuilder propertyBuilder = new();
            StringBuilder chainMethodsBuilder = new();
            var awaitable = false;
            propertyBuilder.AppendLine("        {");
            propertyBuilder.AppendLine($"            var property_result = new PropertyResult(serviceProvider, nameof({member.Name}));");
            foreach (var group in groupedAttributes)
            {

                var infos = group.ToList();
                var chainMethod = group.Key switch
                {
                    "" => $"Default@Validate@{member.Name}".ToPascalCase(),
                    _ => $"Validate@{member.Name}@{group.Key}".ToPascalCase()
                };
                if (GeneratePropertyChainMethod(chainMethodsBuilder, member, chainMethod, group.Key, infos))
                {
                    chainMethod = "await " + chainMethod;
                    awaitable = true;
                }
                propertyBuilder.AppendLine($"            property_result.AddChainResult({chainMethod}(serviceProvider));");


            }
            if (member.NestedConfig != null)
            {
                if (member.NestedConfig.IsAsync)
                {
                    awaitable = true;
                    propertyBuilder.AppendLine($"            if ({member.Name} != null) await property_result.AddNestedResultAsync({member.Name});");
                }
                else
                    propertyBuilder.AppendLine($"            if ({member.Name} != null) property_result.AddNestedResult({member.Name});");
            }
            propertyBuilder.AppendLine("            return property_result;");
            propertyBuilder.AppendLine("        }");
            propertyBuilder.AppendLine();
            if (awaitable)
                sb.AppendLine($"        public async ValueTask<IPropertyResult> {methodName}(IServiceProvider serviceProvider)");
            else
                sb.AppendLine($"        public IPropertyResult {methodName}(IServiceProvider serviceProvider)");
            sb.Append(propertyBuilder);
            sb.Append(chainMethodsBuilder);
            return awaitable;

        }




        private bool GeneratePropertyChainMethod(StringBuilder sb, MemberInfo member, string methodName, string chain, List<AttributeInfo> infos)
        {
            var passedChainValue = chain switch
            {
                null => "string.Empty",
                "" => "string.Empty",
                _ => $"\"{chain}\""
            };
            var propsBuilder = new StringBuilder();
            var awaitable = _processor.ProcessPropertyValidation(propsBuilder, member, "result", "result", "serviceProvider", infos);
            var returnType = awaitable ? "async ValueTask<IChainResult>" : "IChainResult";
            sb.AppendLine($"        public {returnType} {methodName}(IServiceProvider serviceProvider)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = new ChainResult(serviceProvider, {passedChainValue}, nameof({member.Name}));");
            sb.Append(propsBuilder);
            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            return awaitable;
        }


        private string GetChainValue(AttributeInfo attr) => attr.Chain;
    }
}
