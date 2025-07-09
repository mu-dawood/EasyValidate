using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;
using EasyValidate.ConsoleTest;


BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, DefaultConfig
.Instance);
