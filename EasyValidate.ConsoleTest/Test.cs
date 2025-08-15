using EasyValidate.Abstractions;
using EasyValidate.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace EasyValidate.ConsoleTest;




public partial class Model : IGenerate
{
    [NotNull, NotEmpty]
    public string? Name { get; set; }
    private string Private([Length(10)] string name, [GreaterThan(10)] int age)
    {
        return $"Name: {name}, Age: {age}";
        // Private method logic here
    }
}

public class Test
{
    public static async Task Main(string[] args)
    {
        var user = new Model();
        user.Validate();
        var result = user.Private("John Doe", 25);
        if (result.IsValid())
        {
            Console.WriteLine("User updated successfully.", result.Result);
        }

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



/// Create a reusable custom attribute for email existence
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class EmailExistsAttribute : Attribute, IAsyncValidationAttribute<string>
{
    public string ErrorCode => "EmailExists";

    public string? ConditionalMethod { get; set; }

    public string Chain { get; set; } = string.Empty;

    public IServiceProvider? ServiceProvider { get; init; }

    public async Task<AttributeResult> ValidateAsync(string propertyName, string value)
    {
        // Simulate an async check for email existence
        // var db = ServiceProvider.GetService<DBContext>();
        // var exists = await db.Users.AnyAsync(u => u.Email == value);
        return AttributeResult.Success(); // Simulate success for demonstration
    }
}


public partial class User : IGenerate
{
    public bool IsValid() => true;
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

