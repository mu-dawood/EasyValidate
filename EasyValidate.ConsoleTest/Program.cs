using BenchmarkDotNet.Running;
using EasyValidate.Benchmarks;


BenchmarkSwitcher
    .FromAssembly(typeof(Program).Assembly)
    .Run(args);
