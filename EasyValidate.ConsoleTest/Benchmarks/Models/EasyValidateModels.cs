using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Benchmarks.Models.EasyValidate
{
    public partial class SimpleUser : IValidate
    {
        [NotNull]
        [MaxLength(50)]
        public string Name { get; set; } = "John Doe";

        [NotNull]
        [MinLength(5)]
        public string Email { get; set; } = "abcde";

        [Range(18, 120)]
        public int Age { get; set; } = 30;
    }

    public partial class HeavyUser : IValidate
    {
        [NotNull]
        [MaxLength(50)]
        public string Name { get; set; } = "John Doe";

        [NotNull]
        [MinLength(5)]
        public string Email { get; set; } = "abcde";

        [Range(18, 120)]
        public int Age { get; set; } = 30;

        [NotNull]
        [MaxLength(100)]
        public string Address { get; set; } = "123 Main St";

        [NotNull]
        [Matches(@"\+\d{1,3}\s?\d{4,14}")]
        public string Phone { get; set; } = "+1234567890";

        [NotNull]
        [MaxLength(50)]
        public string JobTitle { get; set; } = "Software Developer";

        [NotNull]
        [MaxLength(50)]
        public string Department { get; set; } = "IT";

        [NotNull]
        public string Manager { get; set; } = "Jane Doe";

        [Range(0, 50)]
        public int YearsExperience { get; set; } = 5;

        [Range(30000, 200000)]
        public decimal Salary { get; set; } = 60000;
    }

    public partial class Address
: IValidate
    {
        [NotNull]
        [MaxLength(100)]
        public string Street { get; set; } = "123 Main St";

        [NotNull]
        [MaxLength(50)]
        public string City { get; set; } = "Metropolis";

        [NotNull]
        [MaxLength(50)]
        public string Country { get; set; } = "Countryland";
        public void X()
        {
            // this.Validate(); // This is just to ensure the class is validated
        }
    }

    public partial class HeavyUserWithNested
: IValidate
    {
        [NotNull]
        [MaxLength(50)]
        public string Name { get; set; } = "John Doe";

        [NotNull]
        [MinLength(5)]
        public string Email { get; set; } = "abcde";

        [Range(18, 120)]
        public int Age { get; set; } = 30;

        [NotNull]
        public Address Address { get; set; } = new Address();

        [NotNull]
        [MaxLength(50)]
        public string JobTitle { get; set; } = "Software Developer";

        [NotNull]
        [MaxLength(50)]
        public string Department { get; set; } = "IT";

        [NotNull]
        public string Manager { get; set; } = "Jane Doe";

        [Range(0, 50)]
        public int YearsExperience { get; set; } = 5;

        [Range(30000, 200000)]
        public decimal Salary { get; set; } = 60000;

    }
}
