using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Phone
{
    public  partial class PhoneModel
 : IValidate
    {
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;

        [Phone]
        public string EmergencyContact { get; set; } = string.Empty;

        [Optional, Phone]
        public string? OptionalPhone { get; set; }
    }

    public partial class PhoneNestedModel
 : IValidate
    {
        [Phone]
        public string PrimaryPhone { get; set; } = string.Empty;

        public PhoneModel? NestedPhones { get; set; }
    }
}
