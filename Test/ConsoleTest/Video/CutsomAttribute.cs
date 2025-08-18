
using EasyValidate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace ConsoleTest.Video;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class IsLowerCaseAttribute : Attribute, IValidationAttribute<string>
{
    /// Force ErrorCode to be not configurable
    public string ErrorCode => "LowercaseValidationError";

    ///  Can be configurable
    public string? ConditionalMethod { get; set; }

    public string Chain { get; set; } = string.Empty;

    public AttributeResult Validate(string propertyName, string value)
    {
        bool isValid = value.ToLowerInvariant().Equals(value);
        return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be lowercase.", propertyName);
    }
}



[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class IsLowerCaseAsyncAttribute : Attribute, IAsyncValidationAttribute<string>
{
    /// Force ErrorCode to be not configurable
    public string ErrorCode => "LowercaseValidationError";

    ///  Can be configurable
    public string? ConditionalMethod { get; set; }

    public string Chain { get; set; } = string.Empty;
     public IServiceProvider? ServiceProvider { get; init; }

    public Task<AttributeResult> ValidateAsync(string propertyName, string value)
    {
        var localizer = ServiceProvider?.GetService<IStringLocalizer<IsLowerCaseAsyncAttribute>>();
        bool isValid = value.ToLowerInvariant().Equals(value);
        var res = isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be lowercase.", propertyName);
        return Task.FromResult(res);
    }
}





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