using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ConsoleTest.Benchmarks;
using BenchmarkDotNet.Jobs;
using ConsoleTest;

// var config = ManualConfig.Create(DefaultConfig.Instance)
//     .AddJob(Job.ShortRun.WithLaunchCount(1)
//                 .WithWarmupCount(1)
//                 .WithIterationCount(1));

BenchmarkRunner.Run<FluentFriendlyValidationBenchmarks>();


// var user = new TimingUser(false);
// user.RunNonInvalid();
// user.RunValid();
// user.EnableLogging();
// user.RunNonInvalid();
// user.RunValid();

