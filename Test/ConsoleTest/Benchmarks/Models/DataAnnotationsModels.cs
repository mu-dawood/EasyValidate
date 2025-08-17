using System.ComponentModel.DataAnnotations;

namespace ConsoleTest.Benchmarks.Models.DataAnnotations
{
    public class SimpleUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "John Doe";

        [Required]
        [MinLength(5)]
        public string Email { get; set; } = "abcde";

        [Range(18, 120)]
        public int Age { get; set; } = 30;
    }

    public class HeavyUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "John Doe";

        [Required]
        [MinLength(5)]
        public string Email { get; set; } = "abcde";

        [Range(18, 120)]
        public int Age { get; set; } = 30;

        [Required]
        [MaxLength(100)]
        public string Address { get; set; } = "123 Main St";

        [Required]
        [RegularExpression(@"\+\d{1,3}\s?\d{4,14}")]
        public string Phone { get; set; } = "+1234567890";

        [Required]
        [MaxLength(50)]
        public string JobTitle { get; set; } = "Software Developer";

        [Required]
        [MaxLength(50)]
        public string Department { get; set; } = "IT";

        [Required]
        public string Manager { get; set; } = "Jane Doe";

        [Range(0, 50)]
        public int YearsExperience { get; set; } = 5;

        [Range(30000, 200000)]
        public decimal Salary { get; set; } = 60000;
    }

    public class Address
    {
        [Required]
        [MaxLength(100)]
        public string Street { get; set; } = "123 Main St";

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = "Metropolis";

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = "Countryland";
    }

    public class HeavyUserWithNested
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = "John Doe";

        [Required]
        [MinLength(5)]
        public string Email { get; set; } = "abcde";

        [Range(18, 120)]
        public int Age { get; set; } = 30;

        [Required]
        public Address Address { get; set; } = new Address();

        [Required]
        [MaxLength(50)]
        public string JobTitle { get; set; } = "Software Developer";

        [Required]
        [MaxLength(50)]
        public string Department { get; set; } = "IT";

        [Required]
        public string Manager { get; set; } = "Jane Doe";

        [Range(0, 50)]
        public int YearsExperience { get; set; } = 5;

        [Range(30000, 200000)]
        public decimal Salary { get; set; } = 60000;
    }
}
