using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyValidate.Generator.Handlers
{
    internal class ClassDeclarationHandler : ValidationHandlerBase
    {
        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);
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


            List<string> interfaces = [];
            if (p.Target.AwaitableMembers.Any())
                interfaces.Add("EasyValidate.Abstractions.IAsyncValidate");
            else
                interfaces.Add("EasyValidate.Abstractions.IValidate");

            /// add interfaces
            if (interfaces.Any())
            {
                modifiers.Append(" : ");
                modifiers.Append(string.Join(", ", interfaces));
            }

            sb.AppendLine($"    {modifiers}");
            sb.AppendLine("    {");

            sb.Append(nextsp);

            sb.AppendLine("    }");
            return (sb, p);
        }
    }
}
