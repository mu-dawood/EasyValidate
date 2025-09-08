using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyValidate.Handlers
{
    internal class ClassDeclarationHandler : ValidationHandlerBase
    {
        internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        {
            var (nextsp, p) = base.Next(@params);

            var sb = new StringBuilder();
            var t = @params.TypeSymbol;

            // ---- build header ----
            var header = BuildPartialHeader(t, p);

            sb.AppendLine($"    {header}");
            sb.AppendLine("    {");
            sb.Append(nextsp);
            sb.AppendLine("    }");

            return (sb, p);
        }


        private static string BuildPartialHeader(INamedTypeSymbol t, HandlerParams p)
        {
            var accessibility = t.DeclaredAccessibility switch
            {
                Accessibility.Public => "public ",
                Accessibility.Internal => "internal ",
                Accessibility.Private => "private ",
                Accessibility.Protected => "protected ",
                Accessibility.ProtectedAndInternal => "private protected ",
                Accessibility.ProtectedOrInternal => "protected internal ",
                _ => string.Empty
            };

            // modifiers (order: static | abstract | sealed) for classes/interfaces
            var mods = new StringBuilder(accessibility);
            if (t.TypeKind == TypeKind.Class && t.IsStatic) mods.Append("static ");
            else
            {
                if (t.IsAbstract && t.TypeKind is TypeKind.Class or TypeKind.Interface) mods.Append("abstract ");
                if (t.IsSealed && t.TypeKind is TypeKind.Class or TypeKind.Interface) mods.Append("sealed ");
            }

            // kind keyword
            var kind = GetKindKeyword(t);

            // name + type params
            var name = t.Name;
            if (t.TypeParameters.Length > 0)
            {
                name += "<" + string.Join(", ", t.TypeParameters.Select(tp => tp.Name)) + ">";
            }

            // base list: ONLY add if no part already declares a base list (partial rule)
            var baseList = string.Empty;
            var interfacesToAdd = GetInjectedInterfaces(p);
            if (interfacesToAdd.Count > 0 && !AnyPartHasBaseList(t))
            {
                baseList = " : " + string.Join(", ", interfacesToAdd.Distinct());
            }

            // where clauses (safe to repeat on any partial)
            var whereClauses = GetWhereClauses(t);

            return $"{mods}partial {kind} {name}{baseList}{whereClauses}".TrimEnd();
        }

        private static string GetKindKeyword(INamedTypeSymbol t)
        {
            if (t.IsRecord)
            {
                return t.TypeKind == TypeKind.Struct
                    ? $"{(t.IsReadOnly ? "readonly " : "")}{(t.IsRefLikeType ? "ref " : "")}record struct"
                    : "record class";
            }

            return t.TypeKind switch
            {
                TypeKind.Struct => $"{(t.IsReadOnly ? "readonly " : "")}{(t.IsRefLikeType ? "ref " : "")}struct",
                TypeKind.Interface => "interface",
                TypeKind.Class => "class",
                _ => "class" // fallback: you don't generate for enums/delegates here
            };
        }

        private static bool AnyPartHasBaseList(INamedTypeSymbol t)
            => t.DeclaringSyntaxReferences.Any(r =>
                   r.GetSyntax() is TypeDeclarationSyntax tds &&
                   tds.BaseList is { Types.Count: > 0 });

        private static List<string> GetInjectedInterfaces(HandlerParams p)
        {
            var list = new List<string>();
            if (p.Target.Members.Any())
            {
                if (p.Target.AwaitableMembers.Any())
                    list.Add("IAsyncValidate");
                else
                    list.Add("IValidate");
            }

            // TODO: add more interfaces here if needed (e.g., by config/attributes)
            return list;
        }

        private static string GetWhereClauses(INamedTypeSymbol t)
        {
            if (t.TypeParameters.Length == 0) return string.Empty;

            var clauses = new List<string>(t.TypeParameters.Length);
            foreach (var tp in t.TypeParameters)
            {
                var parts = new List<string>(4);

                if (tp.HasUnmanagedTypeConstraint) parts.Add("unmanaged");
                else if (tp.HasValueTypeConstraint) parts.Add("struct");
                else if (tp.HasReferenceTypeConstraint) parts.Add("class");

                if (tp.HasNotNullConstraint) parts.Add("notnull");

                foreach (var ct in tp.ConstraintTypes)
                {
                    // Minimal formatting is fine here; if you prefer global:: use FullyQualifiedFormat
                    parts.Add(ct.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat
                        .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers)));
                }

                if (tp.HasConstructorConstraint) parts.Add("new()");

                if (parts.Count > 0)
                    clauses.Add($"where {tp.Name} : {string.Join(", ", parts)}");
            }

            return clauses.Count == 0 ? string.Empty : " " + string.Join(" ", clauses);
        }

        // internal override (StringBuilder, HandlerParams) Next(HandlerParams @params)
        // {
        //     var (nextsp, p) = base.Next(@params);
        //     var sb = new StringBuilder();
        //     var typeSymbol = @params.TypeSymbol;
        //     string accessibility = typeSymbol.DeclaredAccessibility switch
        //     {
        //         Accessibility.Public => "public ",
        //         Accessibility.Internal => "internal ",
        //         Accessibility.Private => "private ",
        //         Accessibility.Protected => "protected ",
        //         Accessibility.ProtectedAndInternal => "protected internal ",
        //         Accessibility.ProtectedOrInternal => "private protected ",
        //         _ => string.Empty
        //     };
        //     var modifiers = new StringBuilder(accessibility);
        //     if (typeSymbol.IsStatic)
        //     {
        //         modifiers.Append("static ");
        //     }
        //     else if (typeSymbol.IsAbstract)
        //     {
        //         modifiers.Append("abstract ");
        //     }
        //     modifiers.Append("partial class ");
        //     modifiers.Append(typeSymbol.Name);
        //     if (typeSymbol.TypeParameters.Length > 0)
        //     {
        //         modifiers.Append("<");
        //         modifiers.Append(string.Join(", ", typeSymbol.TypeParameters.Select(tp => tp.Name)));
        //         modifiers.Append(">");
        //     }

        //     if (@params.Target.Members.Any())
        //     {
        //         List<string> interfaces = [];
        //         if (p.Target.AwaitableMembers.Any())
        //             interfaces.Add("IAsyncValidate");
        //         else
        //             interfaces.Add("IValidate");

        //         // add interfaces
        //         if (interfaces.Any())
        //         {
        //             modifiers.Append(" : ");
        //             modifiers.Append(string.Join(", ", interfaces));
        //         }
        //     }

        //     sb.AppendLine($"    {modifiers}");
        //     sb.AppendLine("    {");

        //     sb.Append(nextsp);

        //     sb.AppendLine("    }");
        //     return (sb, p);
        // }
    }
}
