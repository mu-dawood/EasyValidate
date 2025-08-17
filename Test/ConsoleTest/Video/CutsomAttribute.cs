
using EasyValidate.Abstractions;
using System;

namespace ConsoleTest.Video;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class CutsomAttribute : Attribute, IValidationAttribute<string>
{
    public string ErrorCode => throw new NotImplementedException();

    public string? ConditionalMethod => throw new NotImplementedException();

    public string Chain => throw new NotImplementedException();

    public AttributeResult Validate(string propertyName, string value)
    {
        throw new NotImplementedException();
    }
}