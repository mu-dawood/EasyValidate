namespace ConsoleTest.Benchmarks.Models.FluentValidation
{
    public class SimpleUser
    {
        public string Name { get; set; } = "John Doe";
        public string Email { get; set; } = "abcde";
        public int Age { get; set; } = 30;
    }

    public class HeavyUser
    {
        public string Name { get; set; } = "John Doe";
        public string Email { get; set; } = "abcde";
        public int Age { get; set; } = 30;
        public string Address { get; set; } = "123 Main St";
        public string Phone { get; set; } = "+1234567890";
        public string JobTitle { get; set; } = "Software Developer";
        public string Department { get; set; } = "IT";
        public string Manager { get; set; } = "Jane Doe";
        public int YearsExperience { get; set; } = 5;
        public decimal Salary { get; set; } = 60000;
    }

    public class Address
    {
        public string Street { get; set; } = "123 Main St";
        public string City { get; set; } = "Metropolis";
        public string Country { get; set; } = "Countryland";
    }

    public class HeavyUserWithNested
    {
        public string Name { get; set; } = "John Doe";
        public string Email { get; set; } = "abcde";
        public int Age { get; set; } = 30;
        public Address Address { get; set; } = new Address();
        public string JobTitle { get; set; } = "Software Developer";
        public string Department { get; set; } = "IT";
        public string Manager { get; set; } = "Jane Doe";
        public int YearsExperience { get; set; } = 5;
        public decimal Salary { get; set; } = 60000;
    }
}
