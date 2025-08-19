
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





[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class NumericAttribute : Attribute, IValidationAttribute<string, double>
{


    /// <summary>
    /// Gets or sets the name of the validation chain this attribute belongs to.
    /// </summary>
    public string Chain { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return bool. If null or empty, validation always executes.
    /// </summary>
    public string? ConditionalMethod { get; set; }
    /// <summary>
    /// Gets or sets the error code for this validation attribute.
    /// </summary>
    public string ErrorCode { get; set; } = "NumericValidationError";


    /// <inheritdoc/>
    public AttributeResult<double> Validate(string propertyName, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return AttributeResult.Fail<double>("The {0} field must contain only numeric characters.", [propertyName]);
        }
        bool isValid = IsNumeric(value, out double output);
        return isValid
            ? AttributeResult.Success(output)
            : AttributeResult.Fail<double>("The {0} field must contain only numeric characters.", [propertyName]);
    }

    /// <summary>
    /// Checks if the string can be parsed as a number (int, double, decimal, etc).
    /// </summary>
    private static bool IsNumeric(string s, out double number)
    {
        return double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);
    }
}
