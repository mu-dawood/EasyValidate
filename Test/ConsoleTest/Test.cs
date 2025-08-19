using System.Threading.Tasks;
using EasyValidate.Abstractions;
using EasyValidate.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using ConsoleTest;

namespace ConsoleTest;






public enum Gender { Male, Female }

public partial class Model : IGenerate
{
    public Gender Gender { get; set; }

    [NotEmpty, OneOf("Ahmed", "Mohamed", ConditionalMethod = "IsMale")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Determines whether validation should be performed.
    /// </summary>
    /// <param name="result">The current validation result.</param>
    /// <returns>True if validation should be performed; otherwise, false.</returns>
    private bool IsMales(EasyValidate.Abstractions.IChainResult result)
    {
        return true;
    }
    /// <summary>
    /// Determines whether validation should be performed.
    /// </summary>
    /// <param name="result">The current validation result.</param>
    /// <returns>True if validation should be performed; otherwise, false.</returns>
    private bool IsMale(EasyValidate.Abstractions.IChainResult result)
    {
        return true;
    }
}







public class Test
{
    public static async Task Main(string[] args)
    {

        var result = new Model();
        result.Validate();

    }
}

public interface IAgeProvider
{
    int GetPeronAge();
}

public abstract class CarDetails
{
    public string Model { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public int Year { get; set; }
}



[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class IsOld : Attribute, IValidationAttribute<int>
{
    /// Force ErrorCode to be not configurable
    public string ErrorCode => "OldValidationError";

    ///  Can be configurable
    public string? ConditionalMethod { get; set; }

    public string Chain { get; set; } = string.Empty;

    [ValidationContext]
    public IAgeProvider? AgeProvider { get; set; }

    [ValidationContext]
    public CarDetails? CarDetails { get; set; }

    public AttributeResult Validate(string propertyName, int value)
    {
        if (value > 60)
        {
            return AttributeResult.Fail("The {0} field must be less than or equal to 60.", propertyName);
        }
        return AttributeResult.Success();
    }
}


/// Create a reusable custom attribute for email existence
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class EmailExistsAttribute : Attribute, IAsyncValidationAttribute<string>
{
    public string ErrorCode => "EmailExists";

    public string? ConditionalMethod { get; set; }

    public string Chain { get; set; } = string.Empty;
    /// Will be  injected
    public IServiceProvider? ServiceProvider { get; init; }

    public async Task<AttributeResult> ValidateAsync(string propertyName, string value)
    {
        // Simulate an async check for email existence
        // var db = ServiceProvider.GetService<DBContext>();
        // var exists = await db.Users.AnyAsync(u => u.Email == value);
        return AttributeResult.Success(); // Simulate success for demonstration
    }
}




public class MyFormatter : IFormatter
{
    private readonly IStringLocalizer<MyFormatter> _localizer;
    public MyFormatter(IStringLocalizer<MyFormatter> localizer)
    {
        _localizer = localizer;
    }
    public string Format(string messageTemplate, object[] args)
    {
        return _localizer.GetString(messageTemplate, args).Value;
    }
}



public partial class User : IGenerate
{
    public bool IsValid() => true;
    [EmailAddress]
    public string Email { get; private set; } = string.Empty;
    public int? Age { get; private set; }

    private static User Create([EmailAddress] string name, [GreaterThan(18)] int age)
    {
        return new User
        {
            Email = name,
            Age = age
        };
    }

    private void Update([EmailAddress, EmailExists] string name, [GreaterThan(18)] int age)
    {
        Email = name;
        Age = age;
    }

    private Task UpdateAsync([EmailAddress] string name, [GreaterThan(18)] int age)
    {
        Email = name;
        Age = age;
        return Task.CompletedTask;
    }

}

public class Program
{
    public static async Task Main(string[] args)
    {
        /// create is not async,as no awaitable members nor the original method is async
        var result = User.Create("John Doe", 25);
        if (result.IsValid())
        {
            Console.WriteLine($"User created successfully:, Email = {result.Result.Email}, Age = {result.Result.Age}");

            /// update is async, as it has awaitable members
            var updateResult = await result.Result.Update("Jane Doe", 30);

            /// UpdateAsync is async, as the original method is async
            updateResult = await result.Result.UpdateAsync("Jane Doe", 30);
            if (updateResult.IsValid())
                Console.WriteLine($"User updated successfully:, Email = {result.Result.Email}, Age = {result.Result.Age}");
        }

    }
}

