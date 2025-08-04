using EasyValidate.Helpers;
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
    internal class ParameterValidationMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new(compilation);

        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            // Process members
            foreach (var method in @params.Target.Methods)
            {
                List<string> awaitableMembersList = [];

                foreach (var member in method.Parmters)
                {
                    var (awaitable, needServiceProvider) = GeneratePropertyValidationMethod(sb, method, member);
                    if (awaitable)
                    {
                        awaitableMembersList.Add(member.Name);
                    }

                }
            }
            sb.Append(nextsp);
            return (sb, awaitableMembers);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private (bool awaitable, bool needServiceProvider) GeneratePropertyValidationMethod(StringBuilder sb, MethodTarget method, MemberInfo member)
        {

            var groupedAttributes = member.Attributes.GroupBy(GetChainValue).OrderBy(g => string.IsNullOrEmpty(g.Key) ? 1 : 0).ThenBy(g => g.Key).ToList();

            var methodName = $"Validate@{member.Name}@for@{method.Symbol.Name}".ToPascalCase();
            StringBuilder propertyBuilder = new();
            StringBuilder chainMethodsBuilder = new();
            var awaitable = false;
            var needServiceProvider = false;
            propertyBuilder.AppendLine("        {");
            propertyBuilder.AppendLine($"            var property_result = new PropertyResult(config, nameof({member.Name}));");
            foreach (var group in groupedAttributes)
            {

                var infos = group.ToList();
                var chainMethod = group.Key switch
                {
                    "" => $"Default@Validate@{member.Name}@for{method.Symbol.Name}".ToPascalCase(),
                    _ => $"Validate@{member.Name}@for@{method.Symbol.Name}@{group.Key}".ToPascalCase()
                };
                var (chainAwaitable, chainNeedServiceProvider) = GeneratePropertyChainMethod(chainMethodsBuilder, member, chainMethod, group.Key, infos);
                if (chainAwaitable)
                {
                    chainMethod = "await " + chainMethod;
                    awaitable = true;
                }
                if (chainNeedServiceProvider)
                    needServiceProvider = true;

                if (chainNeedServiceProvider)
                    propertyBuilder.AppendLine($"            property_result.AddChainResult({chainMethod}({member.Name}, config));");
                else
                    propertyBuilder.AppendLine($"            property_result.AddChainResult({chainMethod}({member.Name}, config));");


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
                sb.AppendLine($"        private async ValueTask<IPropertyResult> {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            else
                sb.AppendLine($"        private IPropertyResult {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            sb.Append(propertyBuilder);
            sb.Append(chainMethodsBuilder);
            return (awaitable, needServiceProvider);

        }



        private (bool awaitable, bool) GeneratePropertyChainMethod(StringBuilder sb, MemberInfo member, string methodName, string chain, List<AttributeInfo> infos)
        {
            var passedChainValue = chain switch
            {
                null => "string.Empty",
                "" => "string.Empty",
                _ => $"\"{chain}\""
            };
            var propsBuilder = new StringBuilder();
            var (awaitable, needServiceProvider) = _processor.ProcessPropertyValidation(propsBuilder, member, infos);
            var returnType = awaitable ? "async ValueTask<IChainResult>" : "IChainResult";
            sb.AppendLine($"        public {returnType} {methodName}({member.Type.SimplifiedTypeName()} {member.Name}, ValidationConfig? config = null)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var result = new ChainResult(config?.Formatter, {passedChainValue}, nameof({member.Name}));");
            sb.Append(propsBuilder);
            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            return (awaitable, needServiceProvider);
        }



        private string GetChainValue(AttributeInfo attr) => attr.Chain;
    }
}
