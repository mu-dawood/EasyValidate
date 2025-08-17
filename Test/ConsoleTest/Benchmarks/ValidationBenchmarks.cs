using BenchmarkDotNet.Attributes;
using FluentValidation.Results;

namespace ConsoleTest.Benchmarks
{
    [MemoryDiagnoser]
    [BenchmarkCategory("Regular")]
    public partial class ValidationBenchmarks
    {
        [GlobalSetup]
        public void Setup()
        {

        }

        // Simple Valid
        [Benchmark]
        public void DataAnnotations_Simple_Valid()
        {
            var model = new Models.DataAnnotations.SimpleUser();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Simple_Valid()
        {
            var model = new Models.FluentValidation.SimpleUser();
            var validator = new Validators.SimpleUserFluentValidator();
            return validator.Validate(model);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Simple_Valid()
        {
            var model = new Models.EasyValidate.SimpleUser();
            return model.Validate();
        }

        // Simple Invalid
        [Benchmark]
        public void DataAnnotations_Simple_Invalid()
        {
            var model = new Models.DataAnnotations.SimpleUser { Name = null!, Email = "bad", Age = 10 };
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Simple_Invalid()
        {
            var model = new Models.FluentValidation.SimpleUser { Name = null!, Email = "bad", Age = 10 };
            var validator = new Validators.SimpleUserFluentValidator();
            return validator.Validate(model);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Simple_Invalid()
        {
            var model = new Models.EasyValidate.SimpleUser { Name = null!, Email = "bad", Age = 10 };
            return model.Validate();
        }

        // Heavy Valid
        [Benchmark]
        public void DataAnnotations_Heavy_Valid()
        {
            var model = new Models.DataAnnotations.HeavyUser();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Heavy_Valid()
        {
            var model = new Models.FluentValidation.HeavyUser();
            var validator = new Validators.HeavyUserFluentValidator();
            return validator.Validate(model);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Heavy_Valid()
        {
            var model = new Models.EasyValidate.HeavyUser();
            return model.Validate();
        }

        // Heavy Invalid
        [Benchmark]
        public void DataAnnotations_Heavy_Invalid()
        {
            var model = new Models.DataAnnotations.HeavyUser { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Heavy_Invalid()
        {
            var model = new Models.FluentValidation.HeavyUser { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
            var validator = new Validators.HeavyUserFluentValidator();
            return validator.Validate(model);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Heavy_Invalid()
        {
            var model = new Models.EasyValidate.HeavyUser { Name = null!, Email = "bad", Age = 10, Address = null!, Phone = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
            return model.Validate();
        }

        // Nested Valid
        [Benchmark]
        public void DataAnnotations_Nested_Valid()
        {
            var model = new Models.DataAnnotations.HeavyUserWithNested();
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Nested_Valid()
        {
            var model = new Models.FluentValidation.HeavyUserWithNested();
            var validator = new Validators.HeavyUserWithNestedValidator();
            return validator.Validate(model);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Nested_Valid()
        {
            var model = new Models.EasyValidate.HeavyUserWithNested();
            return model.Validate();
        }

        // Nested Invalid
        [Benchmark]
        public void DataAnnotations_Nested_Invalid()
        {
            var model = new Models.DataAnnotations.HeavyUserWithNested { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
            var ctx = new System.ComponentModel.DataAnnotations.ValidationContext(model);
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, ctx, results, true);
        }
        [Benchmark]
        public ValidationResult FluentValidation_Nested_Invalid()
        {
            var model = new Models.FluentValidation.HeavyUserWithNested { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
            var validator = new Validators.HeavyUserWithNestedValidator();
            return validator.Validate(model);
        }
        [Benchmark]
        public EasyValidate.Abstractions.IValidationResult EasyValidate_Nested_Invalid()
        {
            var model = new Models.EasyValidate.HeavyUserWithNested { Name = null!, Email = "bad", Age = 10, Address = null!, JobTitle = null!, Department = null!, Manager = null!, YearsExperience = 100, Salary = -1 };
            return model.Validate();
        }

    }
}
