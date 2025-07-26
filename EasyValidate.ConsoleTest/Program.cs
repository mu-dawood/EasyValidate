using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using EasyValidate.Benchmarks;
using BenchmarkDotNet.Jobs;

// var config = ManualConfig.Create(DefaultConfig.Instance)
//     .AddJob(Job.ShortRun.WithLaunchCount(1)
//                 .WithWarmupCount(1)
//                 .WithIterationCount(1));

BenchmarkRunner.Run<FluentFriendlyValidationBenchmarks>();
