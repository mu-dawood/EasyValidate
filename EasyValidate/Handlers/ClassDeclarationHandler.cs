using EasyValidate.Types;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ClassDeclarationHandler : ValidationHandlerBase
    {
        public override (StringBuilder sb, Dictionary<string, List<string>> awaitableMembers) Next(HandlerParams @params)
        {
            var (nextsp, awaitableMembers) = base.Next(@params);
            var sb = new StringBuilder();
            var classSymbol = @params.ClassSymbol;
            string accessibility = classSymbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public ",
                Accessibility.Internal => "internal ",
                Accessibility.Private => "private ",
                Accessibility.Protected => "protected ",
                Accessibility.ProtectedAndInternal => "protected internal ",
                Accessibility.ProtectedOrInternal => "private protected ",
                _ => string.Empty
            };
            var modifiers = new StringBuilder(accessibility);
            if (classSymbol.IsStatic)
            {
                modifiers.Append("static ");
            }
            else if (classSymbol.IsAbstract)
            {
                modifiers.Append("abstract ");
            }
            modifiers.Append("partial class ");
            modifiers.Append(classSymbol.Name);
            if (classSymbol.TypeParameters.Length > 0)
            {
                modifiers.Append("<");
                modifiers.Append(string.Join(", ", classSymbol.TypeParameters.Select(tp => tp.Name)));
                modifiers.Append(">");
            }


            // List<string> interfaces = [];

            // /// add interfaces
            // if (interfaces.Any())
            // {
            //     modifiers.Append(" : ");
            //     modifiers.Append(string.Join(", ", interfaces));
            // }

            sb.AppendLine($"    {modifiers}");
            sb.AppendLine("    {");

            sb.Append(nextsp);

            sb.AppendLine("    }");
            return (sb, awaitableMembers);
        }
    }
}
