using EasyValidate.Core.Abstraction;
using EasyValidate.Core.Attributes;

namespace EasyValidate.ConsoleTest;

public partial class TestClass : IGenerate
{

    /// You will see:
    /// Method 'TestMethod' is public and has validation attributes. 
    /// This is allowed, but may lead to ambiguity between your original method 
    /// and the generated overload. To ensure the generated method is called, 
    /// pass 'null' or a ValidationConfig object as the last parameter.
    public string TestMethod([NotNull] Dto? name, [NotNull, NotEmpty] string? value)
    {
        return "TestMethod executed";
    }

    private  string TestMethod2([NotNull] Dto? name, [NotNull, NotEmpty] string? value)
    {
        return "TestMethod2 executed";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var dto = new TestClass();
        /// TestMethod will call  original as its public method
        string result = dto.TestMethod(new Dto(), "test value");

        // TestMethod2 will not call generated as original its private method
        IValidationResult<string> result2 = TestClass.TestMethod2(new Dto(), "test value");

        /// you need to pass ValidationConfig or null to the method to call the generated method
        /// There a warning if you make orignal method public
        var validatedResult = dto.TestMethod(new Dto(), "test value", null);
        if (validatedResult.IsValid())
        {
            Console.WriteLine("Validation passed.", validatedResult.Result);
        }
        else
        {
            Console.WriteLine("Validation failed.");
        }
    }
}
