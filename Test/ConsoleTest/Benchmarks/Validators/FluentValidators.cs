using ConsoleTest.Benchmarks.Models.FluentValidation;
using FluentValidation;

namespace ConsoleTest.Benchmarks.Validators
{
    public class SimpleUserFluentValidator : AbstractValidator<SimpleUser>
    {
        public SimpleUserFluentValidator()
        {
            RuleFor(x => x.Name).NotNull().MaximumLength(50);
            RuleFor(x => x.Email).NotNull().MinimumLength(5);
            RuleFor(x => x.Age).InclusiveBetween(18, 120);
        }
    }

    public class HeavyUserFluentValidator : AbstractValidator<HeavyUser>
    {
        public HeavyUserFluentValidator()
        {
            RuleFor(x => x.Name).NotNull().MaximumLength(50);
            RuleFor(x => x.Email).NotNull().MinimumLength(5);
            RuleFor(x => x.Age).InclusiveBetween(18, 120);
            RuleFor(x => x.Address).NotNull().MaximumLength(100);
            // RuleFor(x => x.Phone).NotNull().Matches(@"\+\d{1,3}\s?\d{4,14}");
            RuleFor(x => x.JobTitle).NotNull().MaximumLength(50);
            RuleFor(x => x.Department).NotNull().MaximumLength(50);
            RuleFor(x => x.Manager).NotNull();
            RuleFor(x => x.YearsExperience).InclusiveBetween(0, 50);
            RuleFor(x => x.Salary).InclusiveBetween(30000, 200000);
        }
    }

    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street).NotNull().MaximumLength(100);
            RuleFor(x => x.City).NotNull().MaximumLength(50);
            RuleFor(x => x.Country).NotNull().MaximumLength(50);
        }
    }

    public class HeavyUserWithNestedValidator : AbstractValidator<HeavyUserWithNested>
    {
        public HeavyUserWithNestedValidator()
        {
            RuleFor(x => x.Name).NotNull().MaximumLength(50);
            RuleFor(x => x.Email).NotNull().MinimumLength(5);
            RuleFor(x => x.Age).InclusiveBetween(18, 120);
            RuleFor(x => x.Address).NotNull().SetValidator(new AddressValidator());
            RuleFor(x => x.JobTitle).NotNull().MaximumLength(50);
            RuleFor(x => x.Department).NotNull().MaximumLength(50);
            RuleFor(x => x.Manager).NotNull();
            RuleFor(x => x.YearsExperience).InclusiveBetween(0, 50);
            RuleFor(x => x.Salary).InclusiveBetween(30000, 200000);
        }
    }
}
