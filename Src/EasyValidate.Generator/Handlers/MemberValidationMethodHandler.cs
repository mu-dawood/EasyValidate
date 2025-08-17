using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Generator;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
namespace EasyValidate.Handlers
{
    /// <summary>
    /// Generates individual private validation methods for each property.
    /// </summary>
    internal class MemberValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
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
            @params.Target.SetAwaitableMembers(awaitableMembersList.Distinct());
            sb.Append(nextsp);
            return (sb, p);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private bool GeneratePropertyValidationMethod(StringBuilder sb, Member member)
        {


            var methodName = $"Validate@{member.Name}".ToPascalCase();
            StringBuilder propertyBuilder = new();
            StringBuilder chainMethodsBuilder = new();
            var awaitable = false;
            propertyBuilder.AppendLine("        {");
            propertyBuilder.AppendLine($"            var property_result = new PropertyResult(config, nameof({member.Name}));");
            foreach (var group in member.Attributes)
            {

                var infos = group.Value;
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
                propertyBuilder.AppendLine($"            property_result.AddChainResult({chainMethod}(config));");

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
                sb.AppendLine($"        public async ValueTask<IPropertyResult> {methodName}(ValidationConfig? config = null)");
            else
                sb.AppendLine($"        public IPropertyResult {methodName}(ValidationConfig? config = null)");
            sb.Append(propertyBuilder);
            sb.Append(chainMethodsBuilder);
            return awaitable;

        }




        private bool GeneratePropertyChainMethod(StringBuilder sb, Member member, string methodName, string chain, IReadOnlyCollection<AttributeInfo> infos)
        {
            var passedChainValue = chain switch
            {
                null => "string.Empty",
                "" => "string.Empty",
                _ => $"\"{chain}\""
            };
            var propsBuilder = new StringBuilder();
            var awaitable = _processor.ProcessPropertyValidation(propsBuilder, member, infos);
            var returnType = awaitable ? "async ValueTask<IChainResult>" : "IChainResult";

            sb.AppendLine($"        public {returnType} {methodName}(ValidationConfig? config = null)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = new ChainResult(config?.Formatter, {passedChainValue}, nameof({member.Name}));");
            sb.Append(propsBuilder);
            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            return awaitable;
        }


        private string GetChainValue(AttributeInfo attr) => attr.Chain;
    }
}
