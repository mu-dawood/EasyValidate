using BenchmarkDotNet.Attributes;
using FluentValidation.Results;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public partial class ValidationBenchmarks
    {
        // DataAnnotations
        private EasyValidate.Benchmarks.Models.DataAnnotations.SimpleUser _daSimpleValid = new();
        private EasyValidate.Benchmarks.Models.DataAnnotations.SimpleUser _daSimpleInvalid = new() { Name = null!, Email = "bad", Age = 10 };
        private EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUser _daHeavyValid = new();
        private EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUser _daHeavyInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxSimpleValid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxSimpleInvalid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxHeavyValid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxHeavyInvalid;

        // FluentValidation
        private EasyValidate.Benchmarks.Models.FluentValidation.SimpleUser _fvSimpleValid = new();
        private EasyValidate.Benchmarks.Models.FluentValidation.SimpleUser _fvSimpleInvalid = new() { Name = null!, Email = "bad", Age = 10 };
        private EasyValidate.Benchmarks.Models.FluentValidation.HeavyUser _fvHeavyValid = new();
        private EasyValidate.Benchmarks.Models.FluentValidation.HeavyUser _fvHeavyInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private Validators.SimpleUserFluentValidator _fvSimpleValidator = new();
        private Validators.HeavyUserFluentValidator _fvHeavyValidator = new();

        // EasyValidate
        private EasyValidate.Benchmarks.Models.EasyValidate.SimpleUser _evSimpleValid = new();
        private EasyValidate.Benchmarks.Models.EasyValidate.SimpleUser _evSimpleInvalid = new() { Name = null!, Email = "bad", Age = 10 };
        private EasyValidate.Benchmarks.Models.EasyValidate.HeavyUser _evHeavyValid = new();
        private EasyValidate.Benchmarks.Models.EasyValidate.HeavyUser _evHeavyInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };

        // Nested Valid/Invalid objects
        private EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUserWithNested _daNestedValid = new();
        private EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUserWithNested _daNestedInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxNestedValid;
        private System.ComponentModel.DataAnnotations.ValidationContext? _ctxNestedInvalid;

        private EasyValidate.Benchmarks.Models.FluentValidation.HeavyUserWithNested _fvNestedValid = new();
        private EasyValidate.Benchmarks.Models.FluentValidation.HeavyUserWithNested _fvNestedInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
        private Validators.HeavyUserWithNestedValidator _fvNestedValidator = new();

        private EasyValidate.Benchmarks.Models.EasyValidate.HeavyUserWithNested _evNestedValid = new();
        private EasyValidate.Benchmarks.Models.EasyValidate.HeavyUserWithNested _evNestedInvalid = new() { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };

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
        public void DataAnnotations_Simple_Valid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daSimpleValid, _ctxSimpleValid!, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Simple_Valid()
        {
            return _fvSimpleValidator.Validate(_fvSimpleValid);
        }
        [Benchmark]
        public EasyValidate.Core.Abstraction.IValidationResult EasyValidate_Simple_Valid()
        {
            return _evSimpleValid.Validate();
        }

        // Simple Invalid
        [Benchmark]
        public void DataAnnotations_Simple_Invalid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daSimpleInvalid, _ctxSimpleInvalid!, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Simple_Invalid()
        {
            return _fvSimpleValidator.Validate(_fvSimpleInvalid);
        }
        [Benchmark]
        public EasyValidate.Core.Abstraction.IValidationResult EasyValidate_Simple_Invalid()
        {
            return _evSimpleInvalid.Validate();
        }

        // Heavy Valid
        [Benchmark]
        public void DataAnnotations_Heavy_Valid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daHeavyValid, _ctxHeavyValid!, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Heavy_Valid()
        {
            return _fvHeavyValidator.Validate(_fvHeavyValid);
        }
        [Benchmark]
        public EasyValidate.Core.Abstraction.IValidationResult EasyValidate_Heavy_Valid()
        {
            return _evHeavyValid.Validate();
        }

        // Heavy Invalid
        [Benchmark]
        public void DataAnnotations_Heavy_Invalid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daHeavyInvalid, _ctxHeavyInvalid!, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Heavy_Invalid()
        {
            return _fvHeavyValidator.Validate(_fvHeavyInvalid);
        }
        [Benchmark]
        public EasyValidate.Core.Abstraction.IValidationResult EasyValidate_Heavy_Invalid()
        {
            return _evHeavyInvalid.Validate();
        }

        // Nested Valid
        [Benchmark]
        public void DataAnnotations_Nested_Valid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daNestedValid, _ctxNestedValid!, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Nested_Valid()
        {
            return _fvNestedValidator.Validate(_fvNestedValid);
        }
        [Benchmark]
        public EasyValidate.Core.Abstraction.IValidationResult EasyValidate_Nested_Valid()
        {
            return _evNestedValid.Validate();
        }

        // Nested Invalid
        [Benchmark]
        public void DataAnnotations_Nested_Invalid()
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(_daNestedInvalid, _ctxNestedInvalid!, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Nested_Invalid()
        {
            return _fvNestedValidator.Validate(_fvNestedInvalid);
        }
        [Benchmark]
        public EasyValidate.Core.Abstraction.IValidationResult EasyValidate_Nested_Invalid()
        {
            return _evNestedInvalid.Validate();
        }

        // DataAnnotations
        internal Models.DataAnnotations.SimpleUser DaSimpleValid => _daSimpleValid;
        internal EasyValidate.Benchmarks.Models.DataAnnotations.SimpleUser DaSimpleInvalid => _daSimpleInvalid;
        internal EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUser DaHeavyValid => _daHeavyValid;
        internal EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUser DaHeavyInvalid => _daHeavyInvalid;
        internal System.ComponentModel.DataAnnotations.ValidationContext? CtxSimpleValid => _ctxSimpleValid;
        internal System.ComponentModel.DataAnnotations.ValidationContext? CtxSimpleInvalid => _ctxSimpleInvalid;
        internal System.ComponentModel.DataAnnotations.ValidationContext? CtxHeavyValid => _ctxHeavyValid;
        internal System.ComponentModel.DataAnnotations.ValidationContext? CtxHeavyInvalid => _ctxHeavyInvalid;
        internal EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUserWithNested DaNestedValid => _daNestedValid;
        internal EasyValidate.Benchmarks.Models.DataAnnotations.HeavyUserWithNested DaNestedInvalid => _daNestedInvalid;
        internal System.ComponentModel.DataAnnotations.ValidationContext? CtxNestedValid => _ctxNestedValid;
        internal System.ComponentModel.DataAnnotations.ValidationContext? CtxNestedInvalid => _ctxNestedInvalid;

        // FluentValidation
        internal EasyValidate.Benchmarks.Models.FluentValidation.SimpleUser FvSimpleValid => _fvSimpleValid;
        internal EasyValidate.Benchmarks.Models.FluentValidation.SimpleUser FvSimpleInvalid => _fvSimpleInvalid;
        internal EasyValidate.Benchmarks.Models.FluentValidation.HeavyUser FvHeavyValid => _fvHeavyValid;
        internal EasyValidate.Benchmarks.Models.FluentValidation.HeavyUser FvHeavyInvalid => _fvHeavyInvalid;
        internal Validators.SimpleUserFluentValidator FvSimpleValidator => _fvSimpleValidator;
        internal Validators.HeavyUserFluentValidator FvHeavyValidator => _fvHeavyValidator;
        internal EasyValidate.Benchmarks.Models.FluentValidation.HeavyUserWithNested FvNestedValid => _fvNestedValid;
        internal EasyValidate.Benchmarks.Models.FluentValidation.HeavyUserWithNested FvNestedInvalid => _fvNestedInvalid;
        internal Validators.HeavyUserWithNestedValidator FvNestedValidator => _fvNestedValidator;

        // EasyValidate
        internal EasyValidate.Benchmarks.Models.EasyValidate.SimpleUser EvSimpleValid => _evSimpleValid;
        internal EasyValidate.Benchmarks.Models.EasyValidate.SimpleUser EvSimpleInvalid => _evSimpleInvalid;
        internal EasyValidate.Benchmarks.Models.EasyValidate.HeavyUser EvHeavyValid => _evHeavyValid;
        internal EasyValidate.Benchmarks.Models.EasyValidate.HeavyUser EvHeavyInvalid => _evHeavyInvalid;
        internal EasyValidate.Benchmarks.Models.EasyValidate.HeavyUserWithNested EvNestedValid => _evNestedValid;
        internal EasyValidate.Benchmarks.Models.EasyValidate.HeavyUserWithNested EvNestedInvalid => _evNestedInvalid;

        // --- Return validation results for use in Program.cs ---
        public List<System.ComponentModel.DataAnnotations.ValidationResult> ValidateDataAnnotations(object model, System.ComponentModel.DataAnnotations.ValidationContext ctx)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }
        public FluentValidation.Results.ValidationResult ValidateFluent<T>(FluentValidation.IValidator<T> validator, T model)
        {
            return validator.Validate(model);
        }
        public EasyValidate.Core.Abstraction.IValidationResult ValidateEasy<T>(T model) where T : EasyValidate.Core.Abstraction.IValidate
        {
            return model.Validate();
        }
    }
}
