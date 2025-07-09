using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using EasyValidate.ConsoleTest;


BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, ManualConfig.CreateEmpty()
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .WithOptions(ConfigOptions.JoinSummary)
    .WithOptions(ConfigOptions.KeepBenchmarkFiles)
    .AddLogger(new FocusLogger())
    .HideColumns("Job", "Toolchain", "IterationCount", "MaxWarmupIterationCount", "WarmupCount")
);
