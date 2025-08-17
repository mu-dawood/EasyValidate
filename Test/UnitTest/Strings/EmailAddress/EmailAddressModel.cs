using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.EmailAddress;

public partial class EmailAddressModel
 : IValidate, IGenerate
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [Optional, EmailAddress]
    public string? OptionalEmail { get; set; }
}

public partial class EmailAddressNestedModel
 : IValidate, IGenerate
{
    [EmailAddress]
    public string AdminEmail { get; set; } = string.Empty;

    public EmailAddressModel? UserEmails { get; set; }
}
