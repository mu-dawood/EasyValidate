using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using EasyValidate.Benchmarks;
using EasyValidate.Benchmarks.Models.DataAnnotations;
using EasyValidate.Benchmarks.Models.FluentValidation;
using EasyValidate.Benchmarks.Models.EasyValidate;
using EasyValidate.Benchmarks.Validators;

// Run the benchmarks first
// BenchmarkDotNet.Running.BenchmarkRunner.Run<ValidationBenchmarks>();

// Then log the results
Console.WriteLine("\nValidation result comparison for all approaches (valid/invalid, simple/heavy/nested):\n");

var benchmarks = new ValidationBenchmarks();
benchmarks.Setup();

void PrintResult(string label, object result)
{
    if (result is FluentValidation.Results.ValidationResult fv)
        Console.WriteLine($"{label}: {(fv.IsValid ? "Valid" : "Invalid")}, Errors: {fv.Errors.Count}");
    else if (result is IList<System.ComponentModel.DataAnnotations.ValidationResult> da)
        Console.WriteLine($"{label}: {(da.Count == 0 ? "Valid" : "Invalid")}, Errors: {da.Count}");
    else if (result is EasyValidate.Core.Abstraction.IValidationResult ev)
        Console.WriteLine($"{label}: {(ev.IsValid() ? "Valid" : "Invalid")}, Errors: {ev.Errors.Count}");
    else
        Console.WriteLine($"{label}: Unknown result type");
}

// Simple Valid
PrintResult("DataAnnotations_Simple_Valid", benchmarks.ValidateDataAnnotations(benchmarks.DaSimpleValid, benchmarks.CtxSimpleValid!));
PrintResult("FluentValidation_Simple_Valid", benchmarks.ValidateFluent(benchmarks.FvSimpleValidator, benchmarks.FvSimpleValid));
PrintResult("EasyValidate_Simple_Valid", benchmarks.ValidateEasy(benchmarks.EvSimpleValid));

// Simple Invalid
PrintResult("DataAnnotations_Simple_Invalid", benchmarks.ValidateDataAnnotations(benchmarks.DaSimpleInvalid, benchmarks.CtxSimpleInvalid!));
PrintResult("FluentValidation_Simple_Invalid", benchmarks.ValidateFluent(benchmarks.FvSimpleValidator, benchmarks.FvSimpleInvalid));
PrintResult("EasyValidate_Simple_Invalid", benchmarks.ValidateEasy(benchmarks.EvSimpleInvalid));

// Heavy Valid
PrintResult("DataAnnotations_Heavy_Valid", benchmarks.ValidateDataAnnotations(benchmarks.DaHeavyValid, benchmarks.CtxHeavyValid!));
PrintResult("FluentValidation_Heavy_Valid", benchmarks.ValidateFluent(benchmarks.FvHeavyValidator, benchmarks.FvHeavyValid));
PrintResult("EasyValidate_Heavy_Valid", benchmarks.ValidateEasy(benchmarks.EvHeavyValid));

// Heavy Invalid
PrintResult("DataAnnotations_Heavy_Invalid", benchmarks.ValidateDataAnnotations(benchmarks.DaHeavyInvalid, benchmarks.CtxHeavyInvalid!));
PrintResult("FluentValidation_Heavy_Invalid", benchmarks.ValidateFluent(benchmarks.FvHeavyValidator, benchmarks.FvHeavyInvalid));
PrintResult("EasyValidate_Heavy_Invalid", benchmarks.ValidateEasy(benchmarks.EvHeavyInvalid));

// Nested Valid
PrintResult("DataAnnotations_Nested_Valid", benchmarks.ValidateDataAnnotations(benchmarks.DaNestedValid, benchmarks.CtxNestedValid!));
PrintResult("FluentValidation_Nested_Valid", benchmarks.ValidateFluent(benchmarks.FvNestedValidator, benchmarks.FvNestedValid));
PrintResult("EasyValidate_Nested_Valid", benchmarks.ValidateEasy(benchmarks.EvNestedValid));

// Nested Invalid
PrintResult("DataAnnotations_Nested_Invalid", benchmarks.ValidateDataAnnotations(benchmarks.DaNestedInvalid, benchmarks.CtxNestedInvalid!));
PrintResult("FluentValidation_Nested_Invalid", benchmarks.ValidateFluent(benchmarks.FvNestedValidator, benchmarks.FvNestedInvalid));
PrintResult("EasyValidate_Nested_Invalid", benchmarks.ValidateEasy(benchmarks.EvNestedInvalid));