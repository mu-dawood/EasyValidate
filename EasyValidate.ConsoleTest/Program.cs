using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using EasyValidate.ConsoleTest;


BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, DefaultConfig.Instance
    .AddLogger(new BeautifulLogger())
    .HideColumns("Job", "Toolchain", "IterationCount", "MaxWarmupIterationCount", "WarmupCount")
);
