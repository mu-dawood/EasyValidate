using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyValidate.Fixers;

internal static class TypeDeclarationSyntaxExtensions
{
    /// <summary>
    /// Returns true if the type declaration is a class, struct, or record (class/struct).
    /// </summary>
    public static bool IsClassStructOrRecord(this TypeDeclarationSyntax node)
    {
        return node.Kind() switch
        {
            SyntaxKind.ClassDeclaration => true,
            SyntaxKind.StructDeclaration => true,
            SyntaxKind.RecordDeclaration => true,
            SyntaxKind.RecordStructDeclaration => true,
            _ => false
        };
    }

    public static TypeDeclarationSyntax? GetClassStructOrRecord(this IEnumerable<SyntaxNode> nodes)
    {
        foreach (var n in nodes)
        {
            if (n is TypeDeclarationSyntax t && t.IsClassStructOrRecord())
            {
                return t;
            }
        }
        return null;
    }
}
