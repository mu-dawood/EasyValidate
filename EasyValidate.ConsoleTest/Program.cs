using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;


BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, ManualConfig.CreateEmpty()
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .WithOptions(ConfigOptions.JoinSummary)
    .WithOptions(ConfigOptions.KeepBenchmarkFiles)
    .HideColumns("Job", "Toolchain","IterationCount","MaxWarmupIterationCount","WarmupCount")
    );
