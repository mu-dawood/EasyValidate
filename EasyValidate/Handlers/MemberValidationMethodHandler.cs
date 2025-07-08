using Microsoft.CodeAnalysis;
using System;
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

        public override void Handle(HandlerParams @params)
        {
            // Process members
            foreach (var member in @params.Members)
            {
                GeneratePropertyValidationMethod(@params.StringBuilder, member);
            }

            base.Handle(@params);
        }

        /// <summary>
        /// Generates a private validation method for a specific property.
        /// </summary>
        private void GeneratePropertyValidationMethod(StringBuilder sb, MemberInfo member)
        {

            var groupedAttributes = member.Attributes.GroupBy(GetChainValue).OrderBy(g => string.IsNullOrWhiteSpace(g.Key) ? 1 : 0).ThenBy(g => g.Key).ToList();

            var methodName = $"Validate@{member.Name}".ToPascalCase();

            sb.AppendLine($"        private void {methodName}(ValidationResult result)");
            sb.AppendLine("        {");


            foreach (var group in groupedAttributes)
            {
                var passedChainValue = group.Key switch
                {
                    null => "string.Empty",
                    "" => "string.Empty",
                    _ => $"\"{group.Key}\""
                };
                if (string.IsNullOrWhiteSpace(group.Key))
                {
                    sb.AppendLine($"            // Chain: {group.Key}");
                    sb.AppendLine($"            var chain = result.CreateChain(this, {passedChainValue}, nameof({member.Name}));");
                    _processor.ProcessPropertyValidation(sb, member.Type, member.Name, passedChainValue, [.. group]);

                }
                else
                {
                    var method = $"Validate@{member.Name}@{group.Key}".ToPascalCase();
                    sb.AppendLine($"            {method}(result.CreateChain(this, {passedChainValue}, nameof({member.Name})));");
                }
            }
            _processor.ProcessNestedValidation(sb, member);

            sb.AppendLine("        }");
            sb.AppendLine();

            foreach (var group in groupedAttributes)
            {
                if (string.IsNullOrWhiteSpace(group.Key)) continue;
                var chainName = group.Key;
                var infos = group.ToList();

                // Generate method for each chain
                if (!string.IsNullOrEmpty(chainName))
                {
                    GeneratePropertyChainMethod(sb, member.Type, member.Name, chainName, infos);
                }
            }


        }




        private void GeneratePropertyChainMethod(StringBuilder sb, ITypeSymbol type, string memberName, string chainName, List<AttributeInfo> infos)
        {

            var methodName = $"Validate@{memberName}@{chainName}".ToPascalCase();

            sb.AppendLine($"        private void {methodName}(ValidationChain chain)");
            sb.AppendLine("        {");

            // Process all validation logic for this property
            _processor.ProcessPropertyValidation(sb, type, memberName, chainName, infos);
            sb.AppendLine("        }");
            sb.AppendLine();
        }


        private string GetChainValue(AttributeInfo attr)
        {
            var chainProperty = attr.Attribute.NamedArguments.FirstOrDefault(arg => arg.Key == "Chain");
            return chainProperty.Value.Value?.ToString() ?? "";
        }
    }
}
