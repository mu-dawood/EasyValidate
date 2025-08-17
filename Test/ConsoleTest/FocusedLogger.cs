using BenchmarkDotNet.Loggers;
using System.Text.RegularExpressions;

namespace ConsoleTest;

/// <summary>
/// A beautiful, minimal logger for BenchmarkDotNet that:
///  - Only prints essential information: benchmark group headers, result tables, and the global summary.
///  - Suppresses boilerplate/log spam, progress, and diagnostics.
///  - Outputs plain text, always works in CI (GitHub Actions, etc.), and never uses cursor tricks.
///  - Uses simple color for headers and results if the console supports it.
/// </summary>
public class BeautifulLogger : ILogger
{
    public string Id => nameof(BeautifulLogger);
    public int Priority => 0;

    private static readonly Regex GroupHeaderRegex = new(@"^\s*BenchmarkDotNet\.Artifacts[\\/]results[\\/].*\.([^.]+)-report.*\.json", RegexOptions.Compiled);

    public void WriteLine(LogKind logKind, string text)
    {
        // Suppress empty lines and log spam
        if (string.IsNullOrWhiteSpace(text))
            return;

        // Print help and important info
        if (logKind == LogKind.Help || logKind == LogKind.Header)
        {
            WriteWithColor(text, ConsoleColor.Cyan);
            return;
        }

        // Print errors and warnings
        if (logKind == LogKind.Error)
        {
            WriteWithColor(text, ConsoleColor.Red);
            return;
        }
        if (logKind == LogKind.Warning)
        {
            WriteWithColor(text, ConsoleColor.Yellow);
            return;
        }

        // Print result tables and statistics as-is
        if (logKind == LogKind.Result || logKind == LogKind.Statistic)
        {
            WriteWithColor(text, ConsoleColor.Green);
            return;
        }

        // Print group header if detected
        var match = GroupHeaderRegex.Match(text);
        if (match.Success)
        {
            var group = match.Groups[1].Value;
            WriteWithColor($"\n=== {group} ===\n", ConsoleColor.Magenta);
            return;
        }

        // Print global summary at the end
        if (text.StartsWith("Global total time:", StringComparison.Ordinal))
        {
            WriteWithColor("\n" + text + "\n", ConsoleColor.Cyan);
            return;
        }

        // Print only lines that look like benchmark result tables (contain '|')
        if (text.Contains('|'))
        {
            Console.WriteLine(text);
            return;
        }

        // Suppress all other log spam (progress, diagnostics, etc.)
    }

    public void Write(LogKind logKind, string text)
    {
        // Only print table content (lines with '|')
        if (text.Contains('|'))
        {
            Console.Write(text);
        }
    }

    public void WriteLine() { /* ignore vertical whitespace */ }
    public void Flush() { }

    private static void WriteWithColor(string text, ConsoleColor color)
    {
        var prev = Console.ForegroundColor;
        try
        {
            if (Console.IsOutputRedirected)
            {
                // No color in redirected output (e.g. GitHub Actions)
                Console.WriteLine(text);
            }
            else
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = prev;
            }
        }
        catch
        {
            // Fallback: just print
            Console.WriteLine(text);
        }
    }
}