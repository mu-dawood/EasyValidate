using BenchmarkDotNet.Attributes;
using FluentValidation.Results;

namespace ConsoleTest.Benchmarks
{
    [MemoryDiagnoser]
    [BenchmarkCategory("Friendly")]
    public partial class FluentFriendlyValidationBenchmarks
    {
        // DataAnnotations
        private readonly Models.DataAnnotations.SimpleUser _daSimpleValid = new();
        private readonly Models.DataAnnotations.SimpleUser _daSimpleInvalid = new() { Name = null!, Email = "bad", Age = 10 };
        private readonly Models.DataAnnotations.HeavyUser _daHeavyValid = new();
        private readonly Models.DataAnnotations.HeavyUser _daHeavyInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxSimpleValid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxSimpleInvalid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxHeavyValid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxHeavyInvalid;

        // FluentValidation
        private readonly Models.FluentValidation.SimpleUser _fvSimpleValid = new();
        private readonly Models.FluentValidation.SimpleUser _fvSimpleInvalid = new() { Name = null!, Email = "bad", Age = 10 };
        private readonly Models.FluentValidation.HeavyUser _fvHeavyValid = new();
        private readonly Models.FluentValidation.HeavyUser _fvHeavyInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private readonly Validators.SimpleUserFluentValidator _fvSimpleValidator = new();
        private readonly Validators.HeavyUserFluentValidator _fvHeavyValidator = new();

        // EasyValidate
        private readonly Models.EasyValidate.SimpleUser _evSimpleValid = new();
        private readonly Models.EasyValidate.SimpleUser _evSimpleInvalid = new() { Name = null!, Email = "bad", Age = 10 };
        private readonly Models.EasyValidate.HeavyUser _evHeavyValid = new();
        private readonly Models.EasyValidate.HeavyUser _evHeavyInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };

        // Nested Valid/Invalid objects
        private readonly Models.DataAnnotations.HeavyUserWithNested _daNestedValid = new();
        private readonly Models.DataAnnotations.HeavyUserWithNested _daNestedInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxNestedValid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxNestedInvalid;

        private readonly Models.FluentValidation.HeavyUserWithNested _fvNestedValid = new();
        private readonly Models.FluentValidation.HeavyUserWithNested _fvNestedInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private readonly Validators.HeavyUserWithNestedValidator _fvNestedValidator = new();

        private readonly Models.EasyValidate.HeavyUserWithNested _evNestedValid = new();
        private readonly Models.EasyValidate.HeavyUserWithNested _evNestedInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };

        [GlobalSetup]
        public void Setup()
        {
            _ctxSimpleValid = new System.ComponentModel.DataAnnotations.ValidationContext(_daSimpleValid);
            _ctxSimpleInvalid = new System.ComponentModel.DataAnnotations.ValidationContext(_daSimpleInvalid);
            _ctxHeavyValid = new System.ComponentModel.DataAnnotations.ValidationContext(_daHeavyValid);
            _ctxHeavyInvalid = new System.ComponentModel.DataAnnotations.ValidationContext(_daHeavyInvalid);
            _ctxNestedValid = new System.ComponentModel.DataAnnotations.ValidationContext(_daNestedValid);
            _ctxNestedInvalid = new System.ComponentModel.DataAnnotations.ValidationContext(_daNestedInvalid);
        }

        // Simple Valid
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_Simple_Valid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daSimpleValid, _ctxSimpleValid!, results, true);
            return results;
        }
        [Benchmark]
        public ValidationResult FluentValidation_Simple_Valid()
        {
            return _fvSimpleValidator.Validate(_fvSimpleValid);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Simple_Valid()
        {
            return _evSimpleValid.Validate();
        }

        // Simple Invalid
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_Simple_Invalid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daSimpleInvalid, _ctxSimpleInvalid!, results, true);
            return results;
        }
        [Benchmark]
        public ValidationResult FluentValidation_Simple_Invalid()
        {
            return _fvSimpleValidator.Validate(_fvSimpleInvalid);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Simple_Invalid()
        {
            return _evSimpleInvalid.Validate();
        }

        // Heavy Valid
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_Heavy_Valid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daHeavyValid, _ctxHeavyValid!, results, true);
            return results;
        }
        [Benchmark]
        public ValidationResult FluentValidation_Heavy_Valid()
        {
            return _fvHeavyValidator.Validate(_fvHeavyValid);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Heavy_Valid()
        {
            return _evHeavyValid.Validate();
        }

        // Heavy Invalid
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_Heavy_Invalid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daHeavyInvalid, _ctxHeavyInvalid!, results, true);
            return results;
        }
        [Benchmark]
        public ValidationResult FluentValidation_Heavy_Invalid()
        {
            return _fvHeavyValidator.Validate(_fvHeavyInvalid);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Heavy_Invalid()
        {
            return _evHeavyInvalid.Validate();
        }

        // Nested Valid
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_Nested_Valid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daNestedValid, _ctxNestedValid!, results, true);
            return results;
        }
        [Benchmark]
        public ValidationResult FluentValidation_Nested_Valid()
        {
            return _fvNestedValidator.Validate(_fvNestedValid);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Nested_Valid()
        {
            return _evNestedValid.Validate();
        }

        // Nested Invalid
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_Nested_Invalid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daNestedInvalid, _ctxNestedInvalid!, results, true);
            return results;
        }
        [Benchmark]
        public ValidationResult FluentValidation_Nested_Invalid()
        {
            return _fvNestedValidator.Validate(_fvNestedInvalid);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Nested_Invalid()
        {
            return _evNestedInvalid.Validate();
        }

        // --- Modify Then Validate Benchmarks ---
        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_ModifyThenValidate_Simple()
        {
            // Simulate modifying model values before validation
            _daSimpleValid.Name = "John Doe";
            _daSimpleValid.Email = "john@example.com";
            _daSimpleValid.Age = 25;

            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daSimpleValid, _ctxSimpleValid!, results, true);
            return results;
        }

        [Benchmark]
        public ValidationResult FluentValidation_ModifyThenValidate_Simple()
        {
            // Simulate modifying model values before validation
            _fvSimpleValid.Name = "John Doe";
            _fvSimpleValid.Email = "john@example.com";
            _fvSimpleValid.Age = 25;

            return _fvSimpleValidator.Validate(_fvSimpleValid);
        }

        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_ModifyThenValidate_Simple()
        {
            // Simulate modifying model values before validation
            _evSimpleValid.Name = "John Doe";
            _evSimpleValid.Email = "john@example.com";
            _evSimpleValid.Age = 25;

            return _evSimpleValid.Validate();
        }

        [Benchmark]
        public List<System.ComponentModel.DataAnnotations.ValidationResult> DataAnnotations_ModifyThenValidate_Heavy()
        {
            // Simulate modifying model values before validation
            _daHeavyValid.Name = "Jane Smith";
            _daHeavyValid.Email = "jane@company.com";
            _daHeavyValid.Age = 30;
            _daHeavyValid.Phone = "+1-555-0123";
            _daHeavyValid.JobTitle = "Software Engineer";
            _daHeavyValid.Department = "Engineering";
            _daHeavyValid.Salary = 75000;
            _daHeavyValid.YearsExperience = 5;

            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daHeavyValid, _ctxHeavyValid!, results, true);
            return results;
        }

        [Benchmark]
        public ValidationResult FluentValidation_ModifyThenValidate_Heavy()
        {
            // Simulate modifying model values before validation
            _fvHeavyValid.Name = "Jane Smith";
            _fvHeavyValid.Email = "jane@company.com";
            _fvHeavyValid.Age = 30;
            _fvHeavyValid.Phone = "+1-555-0123";
            _fvHeavyValid.JobTitle = "Software Engineer";
            _fvHeavyValid.Department = "Engineering";
            _fvHeavyValid.Salary = 75000;
            _fvHeavyValid.YearsExperience = 5;

            return _fvHeavyValidator.Validate(_fvHeavyValid);
        }

        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_ModifyThenValidate_Heavy()
        {
            // Simulate modifying model values before validation
            _evHeavyValid.Name = "Jane Smith";
            _evHeavyValid.Email = "jane@company.com";
            _evHeavyValid.Age = 30;
            _evHeavyValid.Phone = "+1-555-0123";
            _evHeavyValid.JobTitle = "Software Engineer";
            _evHeavyValid.Department = "Engineering";
            _evHeavyValid.Salary = 75000;
            _evHeavyValid.YearsExperience = 5;

            return _evHeavyValid.Validate();
        }

        // --- Return validation results for use in Program.cs ---
        public List<System.ComponentModel.DataAnnotations.ValidationResult> ValidateDataAnnotations(object model, System.ComponentModel.DataAnnotations.ValidationContext ctx)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }
        public ValidationResult ValidateFluent<T>(FluentValidation.IValidator<T> validator, T model)
        {
            return validator.Validate(model);
        }
        public EasyValidate.Abstractions.IValidationResult ValidateEasy<T>(T model) where T : EasyValidate.Abstractions.IValidate
        {
            return model.Validate();
        }
    }
}
