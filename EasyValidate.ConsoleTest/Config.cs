using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Validators;

namespace EasyValidate.ConsoleTest;
public class MinimalConfig : ManualConfig
{
    public MinimalConfig()
    {
        AddLogger(ConsoleLogger.Default); // or use new CustomLogger() if needed
        WithOptions(ConfigOptions.DisableLogFile); // Optional: disables .log file creation
        AddValidator(ExecutionValidator.FailOnError); // Prevent silent skipping
    }
}