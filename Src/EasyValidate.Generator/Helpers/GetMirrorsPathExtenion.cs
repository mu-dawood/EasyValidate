using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Helpers;

internal static class GetMirrorsPathExtenion
{
    internal static string GetMirroredHintName(this INamedTypeSymbol classSymbol, string? projectDir)
    {
        var syntaxRef = classSymbol.DeclaringSyntaxReferences.FirstOrDefault();
        var filePath = syntaxRef?.SyntaxTree?.FilePath;

        // Fallback: namespace-based layout
        if (filePath == null || string.IsNullOrEmpty(filePath))
        {
            var nsPath = classSymbol.ContainingNamespace?.ToDisplayString()?.Replace('.', '/') ?? "";
            nsPath = nsPath.Trim('/');
            var fallbackDir = string.IsNullOrEmpty(nsPath) ? "" : nsPath + "/";
            return fallbackDir + "/" + classSymbol.Name + ".g.cs";
        }

        // Make project-relative (netstandard2.0-safe)
        var rel = !string.IsNullOrEmpty(projectDir) && IsAbs(filePath)
            ? GetRelativePath(projectDir!, filePath)
            : filePath;

        // Normalize
        rel = rel.Replace('\\', '/').TrimStart('/');

        // Avoid navigating outside (..)
        rel = rel.Replace("..", "__");

        // Drop .cs extension
        string withoutExt = rel.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
            ? rel.Substring(0, rel.Length - 3)
            : rel;

        // One file per *class* in the same mirrored folder
        // Example: A/A/File -> A/A/File_MyClass_Validation.g.cs
        return withoutExt + "/" + classSymbol.Name + ".g.cs";
    }

    private static bool IsAbs(string p)
    {
        // Handles Windows + Unix
        if (string.IsNullOrEmpty(p)) return false;
        if (p.Length >= 2 && char.IsLetter(p[0]) && p[1] == ':') return true; // C:\...
        return p[0] == '/' || p[0] == '\\';
    }

    private static string GetRelativePath(string relativeTo, string path)
    {
        // Uri-based relative path (works in netstandard2.0)
        var fromUri = new Uri(AppendDirSep(relativeTo));
        var toUri = new Uri(path);
        if (!string.Equals(fromUri.Scheme, toUri.Scheme, StringComparison.OrdinalIgnoreCase))
            return path;

        var relativeUri = fromUri.MakeRelativeUri(toUri);
        var relativePath = Uri.UnescapeDataString(relativeUri.ToString());
        if (string.Equals(toUri.Scheme, "file", StringComparison.OrdinalIgnoreCase))
            relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
        return relativePath;
    }

    private static string AppendDirSep(string p)
    {
        if (string.IsNullOrEmpty(p)) return p;
        var sep = Path.DirectorySeparatorChar.ToString();
        return p.EndsWith(sep, StringComparison.Ordinal) ? p : p + sep;
    }

}
