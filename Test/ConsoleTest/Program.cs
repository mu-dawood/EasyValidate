using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ConsoleTest.Benchmarks;
using BenchmarkDotNet.Jobs;
using ConsoleTest;

// var config = ManualConfig.Create(DefaultConfig.Instance)
//     .AddJob(Job.ShortRun.WithLaunchCount(1)
//                 .WithWarmupCount(1)
//                 .WithIterationCount(1));

var benchmarkArg = args.Length > 0 ? args.Last():string.Empty;

switch (benchmarkArg)
{
	case "FluentFriendlyValidationBenchmarks":
		BenchmarkRunner.Run<FluentFriendlyValidationBenchmarks>();
		break;
	case "CollectionBenchmarks":
		BenchmarkRunner.Run<CollectionBenchmarks>();
		break;
	case "DateTimeBenchmarks":
		BenchmarkRunner.Run<DateTimeBenchmarks>();
		break;
	case "GeneralBenchmarks":
		BenchmarkRunner.Run<GeneralBenchmarks>();
		break;
	case "NumericBenchmarks":
		BenchmarkRunner.Run<NumericBenchmarks>();
		break;
	case "OtherBenchmarks":
		BenchmarkRunner.Run<OtherBenchmarks>();
		break;
	case "StringBenchmarks":
		BenchmarkRunner.Run<StringBenchmarks>();
		break;
	case "ValidationBenchmarks":
		BenchmarkRunner.Run<ValidationBenchmarks>();
		break;
	default:
		Console.WriteLine($"Unknown benchmark: {benchmarkArg}. Running FluentFriendlyValidationBenchmarks by default.");
		var x = new ValidationBenchmarks();
x.EasyValidate_Heavy_Valid();
		break;
}

