using EasyValidate.Attributes;
using EasyValidate.Test.Extensions;

using System.Linq;
using EasyValidate.Abstractions;

namespace EasyValidate.Test.ParameterValidation
{
    public partial class TestParameterValidation
 : IGenerate
    {
        public static void TestMethod([NotNull] string name, [Range(1, 100)] int age)
        {
            // This method should have parameter validation generated
            Console.WriteLine($"Name: {name}, Age: {age}");
        }

        /// <summary>
        /// Test method with extra parameters that don't require validation.
        /// The generated overload should accept all parameters but only validate 'name'.
        /// </summary>
        public static string TestMethodWithExtraParams([NotNull] string name, int count, bool enabled)
        {
            // 'name' requires validation, 'count' and 'enabled' are just passed through
            return $"Name: {name}, Count: {count}, Enabled: {enabled}";
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
