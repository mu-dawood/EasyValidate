using BenchmarkDotNet.Running;
using EasyValidate.Benchmarks;


BenchmarkRunner.Run<AttributeSetupBenchmarks>(args: args);
