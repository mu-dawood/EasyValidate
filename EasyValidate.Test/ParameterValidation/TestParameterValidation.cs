using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.ParameterValidation
{
    public partial class TestParameterValidation
    {
        public static void TestMethod([NotNull] string name, [Range(1, 100)] int age)
        {
            // This method should have parameter validation generated
            Console.WriteLine($"Name: {name}, Age: {age}");
        }

        public TestParameterValidation([NotNull] string name, [Range(18, 65)] int age)
        {
            // This constructor should have parameter validation generated
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }
}
