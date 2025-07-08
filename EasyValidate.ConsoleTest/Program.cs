using BenchmarkDotNet.Running;
using EasyValidate.Benchmarks;
using EasyValidate.ConsoleTest;


BenchmarkRunner.Run<AttributeSetupBenchmarks>(new MinimalConfig());


// var benchmark = new FluentFriendlyValidationBenchmarks();
// benchmark.Setup();

// // Test all validation scenarios and show error counts
// Console.WriteLine("\n� VALIDATION ERROR COUNTS BY SCENARIO:");
// Console.WriteLine(new string('-', 50));

// // Simple Valid
// var daSimpleValid = benchmark.DataAnnotations_Simple_Valid();
// var fvSimpleValid = benchmark.FluentValidation_Simple_Valid();
// var evSimpleValid = benchmark.EasyValidate_Simple_Valid();
// Console.WriteLine($"� Simple Valid:     DA={daSimpleValid.Count}, FV={fvSimpleValid.Errors.Count}, EV={evSimpleValid.Errors.Count}");

// // Simple Invalid
// var daSimpleInvalid = benchmark.DataAnnotations_Simple_Invalid();
// var fvSimpleInvalid = benchmark.FluentValidation_Simple_Invalid();
// var evSimpleInvalid = benchmark.EasyValidate_Simple_Invalid();
// Console.WriteLine($"� Simple Invalid:   DA={daSimpleInvalid.Count}, FV={fvSimpleInvalid.Errors.Count}, EV={evSimpleInvalid.Errors.Count}");

// // Heavy Valid
// var daHeavyValid = benchmark.DataAnnotations_Heavy_Valid();
// var fvHeavyValid = benchmark.FluentValidation_Heavy_Valid();
// var evHeavyValid = benchmark.EasyValidate_Heavy_Valid();
// Console.WriteLine($"🟢 Heavy Valid:      DA={daHeavyValid.Count}, FV={fvHeavyValid.Errors.Count}, EV={evHeavyValid.Errors.Count}");

// // Heavy Invalid
// var daHeavyInvalid = benchmark.DataAnnotations_Heavy_Invalid();
// var fvHeavyInvalid = benchmark.FluentValidation_Heavy_Invalid();
// var evHeavyInvalid = benchmark.EasyValidate_Heavy_Invalid();
// Console.WriteLine($"🔴 Heavy Invalid:    DA={daHeavyInvalid.Count}, FV={fvHeavyInvalid.Errors.Count}, EV={evHeavyInvalid.Errors.Count}");

// // Nested Valid
// var daNestedValid = benchmark.DataAnnotations_Nested_Valid();
// var fvNestedValid = benchmark.FluentValidation_Nested_Valid();
// var evNestedValid = benchmark.EasyValidate_Nested_Valid();
// Console.WriteLine($"🟢 Nested Valid:     DA={daNestedValid.Count}, FV={fvNestedValid.Errors.Count}, EV={evNestedValid.Errors.Count}");

// // Nested Invalid
// var daNestedInvalid = benchmark.DataAnnotations_Nested_Invalid();
// var fvNestedInvalid = benchmark.FluentValidation_Nested_Invalid();
// var evNestedInvalid = benchmark.EasyValidate_Nested_Invalid();
// Console.WriteLine($"🔴 Nested Invalid:   DA={daNestedInvalid.Count}, FV={fvNestedInvalid.Errors.Count}, EV={evNestedInvalid.Errors.Count}");

// // Modify Then Validate - Simple
// var daModifySimple = benchmark.DataAnnotations_ModifyThenValidate_Simple();
// var fvModifySimple = benchmark.FluentValidation_ModifyThenValidate_Simple();
// var evModifySimple = benchmark.EasyValidate_ModifyThenValidate_Simple();
// Console.WriteLine($"🔧 Modify Simple:    DA={daModifySimple.Count}, FV={fvModifySimple.Errors.Count}, EV={evModifySimple.Errors.Count}");

// // Modify Then Validate - Heavy
// var daModifyHeavy = benchmark.DataAnnotations_ModifyThenValidate_Heavy();
// var fvModifyHeavy = benchmark.FluentValidation_ModifyThenValidate_Heavy();
// var evModifyHeavy = benchmark.EasyValidate_ModifyThenValidate_Heavy();
// Console.WriteLine($"� Modify Heavy:     DA={daModifyHeavy.Count}, FV={fvModifyHeavy.Errors.Count}, EV={evModifyHeavy.Errors.Count}");

