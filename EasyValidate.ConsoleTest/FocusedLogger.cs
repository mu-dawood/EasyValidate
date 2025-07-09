using System;
using System.Collections.Frozen;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Loggers;

namespace EasyValidate.ConsoleTest;

public partial class FocusLogger : ILogger
{
    public string Id => nameof(FocusLogger);
    public int Priority => 0;

    // The overall strategy of this logger is:
    // - Have two lines at the bottom of the console to indicate current status.
    //     - One line for the number of remaining benchmarks and the estimated finish time.
    //     - One line for the latest output spam from the log.
    //     - These lines update and change, where the default logger would print new lines.
    // - Print only the essentials to persistent output:
    //     - Benchmark group name headers.
    //     - Benchmark results tables.
    //     - The global summary at the very end.
    // This creates a nice focused CLI experience for the benchmarks. BDN is unfortunately not really
    // designed for this, so our code to make this happen is a little hacky.


    // Abstraction of console output to help with the "0-2 persistent lines" concept.
    readonly ConsoleOutputWithPersistentLines Output = new();

    // Used for printing tables.
    LastActionType LastAction = default;
    bool CurrentlyPrintingTable = false;

    // Used once we've detected the last line from the original log that we want to print.
    bool BlockFutureWriteLines = false;

    [GeneratedRegex(@"^\s{2}BenchmarkDotNet\.Artifacts[\\/]results[\\/].*\.([^.]+)-report-full-compressed\.json")]
    private static partial Regex GroupNameRegex();

    public void WriteLine(LogKind logKind, string text)
    {
        if (BlockFutureWriteLines)
            return;

        LastAction = LastActionType.WriteLine;
        var color = GetColor(logKind);

        // In tables, we need WriteLine to work as normal.
        if (CurrentlyPrintingTable)
        {
            Output.WriteLine(text, color);
            return;
        }

        // Sometimes BDN uses WriteLine with a blank string to create vertical whitespace.
        // We handle vertical whitespace ourselves, so we skip that here.
        if (string.IsNullOrEmpty(text))
            return;

        // Help should always be shown to the user. This is the green text, like the interactive BenchmarkSwitcher interface.
        if (logKind == LogKind.Help)
        {
            Output.WriteLine(text, color);
            return;
        }

        // This line is one of the lines saying how much is left in the run. We want to keep it around.
        // These lines look like:
        // `// ** Remained 2 (16.7%) benchmark(s) to run. Estimated finish 2025-05-01 15:42 (0h 0m from now) **`
        if (text.StartsWith("// ** Remained "))
        {
            Output.SetRemaining(text, color);
            return;
        }

        // We want to print the name of a group of benchmarks before the table of those benchmarks, as a header.
        // Unfortunately the only way to get the group name (that I'm aware of) is a log that prints the export directory for the results.
        // These lines look like:
        // `  BenchmarkDotNet.Artifacts/results/LogicWorld.Core.Benchmarks.StructureModificationBenchmarks-report-full-compressed.json`
        var match = GroupNameRegex().Match(text);
        if (match.Success)
        {
            string groupName = match.Groups[1].Value;
            Output.WriteLine(groupName, color);
            Output.WriteLine();
            return;
        }

        // At the very end, there's one line that summarizes the whole thing. We want to print this and end our output here.
        // This line looks like:
        // `Global total time: 00:00:12 (12.85 sec), executed benchmarks: 12`
        if (text.StartsWith("Global total time:"))
        {
            Output.WriteLine(text, color);
            Output.DeletePersistentLines();
            BlockFutureWriteLines = true;
            return;
        }

        // Everything else is log spam, which our logger condenses into a single constantly-changing line.
        Output.SetLatest(text, color);
    }

    // As far as I can tell, Write is really only used by BDN to print tables.
    public void Write(LogKind logKind, string text)
    {
        if (text.Contains('|'))
            CurrentlyPrintingTable = true;

        LastAction = LastActionType.Write;
        var color = GetColor(logKind);
        Output.Write(text, color);
    }

    // This is called at various points in the log to space out parts of the log with blank lines.
    // We don't want to print a blank line in those cases as we have much less log, and we handle newlines ourselves.
    // It is also called while printing tables (in combination with Write).
    // We DO want to print a blank line in those cases, so that the tables are printed correctly.
    public void WriteLine()
    {
        // Two consecutive empty writelines marks the end of a table.
        if (CurrentlyPrintingTable && LastAction == LastActionType.WriteLineEmpty)
        {
            CurrentlyPrintingTable = false;
            Output.WriteLine();
            Output.WriteLine();
        }

        LastAction = LastActionType.WriteLineEmpty;

        if (CurrentlyPrintingTable)
            Output.WriteLine();
    }

    public void Flush()
    {
        // This is called at the very end of BDN printing text.
    }




    static ConsoleColor GetColor(LogKind logKind)
        => LogColors.TryGetValue(logKind, out var color) ? color : DefaultColor;

    const ConsoleColor DefaultColor = ConsoleColor.Gray;

    // These colors are copied from BenchmarkDotNet.Loggers.ConsoleLogger.
    static readonly FrozenDictionary<LogKind, ConsoleColor> LogColors = new Dictionary<LogKind, ConsoleColor>()
    {
        [LogKind.Default] = ConsoleColor.Gray,
        [LogKind.Help] = ConsoleColor.DarkGreen,
        [LogKind.Header] = ConsoleColor.Magenta,
        [LogKind.Result] = ConsoleColor.DarkCyan,
        [LogKind.Statistic] = ConsoleColor.Cyan,
        [LogKind.Info] = ConsoleColor.DarkYellow,
        [LogKind.Error] = ConsoleColor.Red,
        [LogKind.Warning] = ConsoleColor.Yellow,
        [LogKind.Hint] = ConsoleColor.DarkCyan,
    }.ToFrozenDictionary();


    enum LastActionType
    {
        Unknown = 0,
        WriteLine,
        Write,
        WriteLineEmpty,
    }

    class ConsoleOutputWithPersistentLines
    {
        (string, ConsoleColor)? PersistentRemaining;
        (string, ConsoleColor)? PersistentLatest;

        public void SetRemaining(string remaining, ConsoleColor color)
        {
            PersistentRemaining = (remaining, color);
            RedrawPersistentLines();
        }

        public void SetLatest(string latest, ConsoleColor color)
        {
            PersistentLatest = (latest, color);
            RedrawPersistentLines();
        }

        public void DeletePersistentLines()
        {
            PersistentRemaining = null;
            PersistentLatest = null;
            RedrawPersistentLines();
        }

        public void Write(string output, ConsoleColor color)
        {
            ClearPersistentLines();

            var colorBefore = Console.ForegroundColor;

            if (color != Console.ForegroundColor && color != Console.BackgroundColor)
                Console.ForegroundColor = color;

            Console.Write(output);

            Console.ForegroundColor = colorBefore;
            
            RedrawPersistentLines();
        }

        public void WriteLine(string output, ConsoleColor color) 
            => Write(output + "\n", color);

        public void WriteLine()
            => Write("\n", default);


        // The persistent lines are the two full lines below the current cursor position.
        void RedrawPersistentLines()
        {
            ClearPersistentLines();
            (int restoreLeft, int restoreTop) = Console.GetCursorPosition();

            // Write the persistent lines, if any, to the two lines below the cursor position.
            Console.CursorTop += 1;
            Console.CursorLeft = 0;

            int expectedCursorTop = restoreTop + 1;

            WriteLineIfNotNull(PersistentRemaining);
            WriteLineIfNotNull(PersistentLatest);

            // Restore the original cursor position, adjusted in case the terminal window has scrolled.
            int diff = expectedCursorTop - Console.CursorTop;
            restoreTop -= diff;

            Console.SetCursorPosition(restoreLeft, restoreTop);


            void WriteLineIfNotNull((string, ConsoleColor)? line)
            {
                if (line == null)
                    return;
                
                var (text, color) = line.Value;

                // Remove newlines so it doesn't spill over into the next line
                text = text.Replace("\n", "").Replace("\r", "");

                // Truncate so it doesn't spill over into the next line
                if (text.Length > MaxWriteLineWidth)
                    text = text[..MaxWriteLineWidth];

                var colorBefore = Console.ForegroundColor;

                if (color != Console.ForegroundColor && color != Console.BackgroundColor)
                    Console.ForegroundColor = color;

                Console.WriteLine(text);
                expectedCursorTop += 1;

                Console.ForegroundColor = colorBefore;
            }
        }
        static void ClearPersistentLines()
        {
            (int restoreLeft, int restoreTop) = Console.GetCursorPosition();

            // Clear the two lines below the cursor position.
            Console.CursorTop += 1;
            Console.CursorLeft = 0;
            Console.WriteLine(new string(' ', MaxWriteLineWidth));
            Console.WriteLine(new string(' ', MaxWriteLineWidth));

            // Restore the original cursor position, adjusted in case the terminal window has scrolled.
            int expectedCursorTop = restoreTop + 3;
            int diff = expectedCursorTop - Console.CursorTop;
            restoreTop -= diff;

            Console.SetCursorPosition(restoreLeft, restoreTop);
        }

        // The -1 is only necessary in the Windows native terminal.
        // It is not needed in Unix terminals, or in other Windows terminals like PowerShell or the special terminal that Visual Studio launches when you run a project.
        // In and only in the Windows native terminal, writing the full width of the console causes a line wrap, which breaks our output.
        static int MaxWriteLineWidth 
            => Console.WindowWidth - 1;
    }
}