using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EasyValidate;

public static class Utils
{
    
  
    public static string ToSakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (char.IsUpper(c))
            {
                if (i > 0)
                    sb.Append('_');

                sb.Append(char.ToLowerInvariant(c));
            }
            else if (c == ' ')
            {
                if (i > 0 && sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
            }
            else if (c == '-')
            {
                if (i > 0 && sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
            }
            else if (c == '_')
            {
                if (sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
            }
            else if (c == '@')
            {
                if (i > 0 && sb.Length > 0 && sb[sb.Length - 1] != '_')
                    sb.Append('_');
                sb.Append("at_");
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static string ToPascalCase(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var words = name.Split(['_', '@'], StringSplitOptions.RemoveEmptyEntries);
        var result = new StringBuilder(name.Length);

        foreach (var word in words)
        {
            if (word.Length == 0) continue;

            result.Append(char.ToUpperInvariant(word[0]));
            if (word.Length > 1)
                result.Append(word.Substring(1).ToLowerInvariant());
        }

        return result.ToString();
    }



}