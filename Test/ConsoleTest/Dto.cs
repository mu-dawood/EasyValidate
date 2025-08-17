using EasyValidate.Attributes;
using EasyValidate.Abstractions;

namespace ConsoleTest;

public partial class Dto : IGenerate
{

    [Optional, PastDate]
    [NotNull(Chain = "chain1"), PastDate(Chain = "chain1")]
    public DateTime? BirthDate { get; set; }

    // Test case: Simple single attribute for testing
    [NotNull, CCC]
    public string? TestSimple { get; set; }

    // Test case: Additional attribute to trigger analyzer
    [NotNull]
    public string? TestAnalyzer { get; set; }

    // Test case: Incompatible type chain - Numeric outputs double, NotEmpty expects string
    // [Numeric, NotEmpty, NotNull]  // Temporarily commented out to allow build
    public string? TestIncompatibleChain { get; set; }

    // [NotEmpty,NotNull]
    [Optional, CCC]
    public string? Name { get; set; }
    public Dto? NestedDto { get; set; }
    /// <summary>
    /// Determines whether validation should be performed.
    /// </summary>
    /// <param name="result">The current validation result.</param>
    /// <returns>A ValueTask containing true if validation should be performed; otherwise, false.</returns>
    private ValueTask<bool> ShouldValidateStartsWith(IChainResult result)
    {
        this.Validate();
        return new System.Threading.Tasks.ValueTask<bool>(true);
    }

    public string TestMethod([NotNull] Dto? name, [NotNull, NotEmpty] string? value)
    {
        this.TestMethod(name, value, new ValidationConfig());
        return "TestMethod executed";
        // This method is intentionally left empty to demonstrate the use of attributes
        // without any specific logic.
    }




    // private void ValidateBirthdate(ValidationResult result)
    // {
    //     ValidationChain chain = new ValidationChain("chain1", nameof(BirthDate), null, null);
    //     var optional_attribute_result = optional_attribute.Validate(nameof(BirthDate), BirthDate);
    //     if (!optional_attribute_result.IsValid)
    //     {
    //         result.AddResult(optional_attribute_result, optional_attribute.GetType().Name, optional_attribute.ErrorCode, BirthDate, nameof(BirthDate), "chain1");
    //         return;
    //     }
    //     var past_date_attribute_result = past_date_attribute.Validate("BirthDate", BirthDate);
    //     if (!past_date_attribute_result.IsValid)
    //     {
    //         return;
    //     }
    // }

}


public class CCC : StringValidationAttributeBase
{
    [ValidationContext]
    public Dto? GG { get; init; }
    public IServiceProvider? ServiceProvider { get; init; }
    public override string ErrorCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override AttributeResult Validate(string propertyName, string value)
    {
        throw new NotImplementedException();
    }
}




// public partial class ProgramTest
// {
//     public static void Main(string[] args)
//     {
//         Console.WriteLine("🧪 TESTING CONDITIONAL VALIDATION WITH OBJECT INITIALIZATION SYNTAX");
//         Console.WriteLine("=================================================================");
//         Console.WriteLine();

//         Console.WriteLine("=== Test Case 1: Empty name (conditional validation should be SKIPPED) ===");
//         var dto1 = new Dto
//         {
//             Name = "", // Empty name - StartsWith validation should be skipped due to conditional method
//         };
//         var result1 = dto1.Validate();
//         PrintValidationResult(result1, "Should only show NotEmpty error, NOT StartsWith error");

//         Console.WriteLine("\n=== Test Case 2: Valid name but doesn't start with 'John' (conditional validation should RUN) ===");
//         var dto2 = new Dto
//         {
//             Name = "Alice", // Valid name but doesn't start with John - StartsWith validation should run
//         };
//         var result2 = dto2.Validate();
//         PrintValidationResult(result2, "Should show StartsWith error because name is valid but doesn't start with 'John'");

//         Console.WriteLine("\n=== Test Case 3: Valid name that starts with 'John' (all validation passes) ===");
//         var dto3 = new Dto
//         {
//             Name = "John Doe", // Valid name that starts with John - all validations should pass
//         };
//         var result3 = dto3.Validate();
//         PrintValidationResult(result3, "Should pass all validations");

//         Console.WriteLine("\n=== Test Case 4: Null name (conditional validation should be SKIPPED) ===");
//         var dto4 = new Dto
//         {
//             Name = null, // Null name - StartsWith validation should be skipped
//         };
//         var result4 = dto4.Validate();
//         PrintValidationResult(result4, "Should only show NotNull error, NOT StartsWith error");

//         Console.WriteLine("\n🎯 SUCCESS: Object initialization syntax working:");
//         Console.WriteLine("   new StartsWithAttribute(\"John\") { ConditionalMethod = \"ShouldValidateStartsWith\" }");
//     }

//     private static void PrintValidationResult(IValidationResult result, string expected)
//     {
//         Console.WriteLine($"Expected: {expected}");

//         if (result.IsValid())
//         {
//             Console.WriteLine("✅ Validation succeeded - No errors found!");
//         }
//         else
//         {
//             Console.WriteLine("❌ Validation failed:");
//             foreach (var errorGroup in result.Errors)
//             {
//                 Console.WriteLine($"   Property '{errorGroup.Key}' has errors:");
//                 foreach (var validationError in errorGroup.Value)
//                 {
//                     Console.WriteLine($"     - {validationError.FormattedMessage}");
//                 }
//             }
//         }
//         Console.WriteLine();
//     }
// }