using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Phone
{
    public partial class PhoneModel
 : IValidate, IGenerate
    {
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;

        [Phone]
        public string EmergencyContact { get; set; } = string.Empty;

        [Optional, Phone]
        public string? OptionalPhone { get; set; }
    }

    public partial class PhoneNestedModel
 : IValidate, IGenerate
    {
        [Phone]
        public string PrimaryPhone { get; set; } = string.Empty;

        public PhoneModel? NestedPhones { get; set; }
    }
}
