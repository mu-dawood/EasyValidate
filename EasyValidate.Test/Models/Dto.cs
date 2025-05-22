


using EasyValidate.Abstraction;
using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.Models
{
    public partial class Dto
    {
        [Required]
        // [Range(18, 99)]
        [IsNotAdmin]
        public string? Name { get; set; }

        [Range(18, 99), LessThan(10)]
        public byte Age { get; set; }

        public string? Email { get; set; }

        public SubDto? SubDto { get; set; }
    }

    public partial class SubDto
    {
        [Range(18, 99)]
        public byte Age { get; set; }

        public SubDto? SubSubDto { get; set; }

    }


    public class IsNotAdmin : ValidationAttributeBase
    {
        public override string ErrorCode => "IsNotAdmin";

        public AttributeResult Validate(string propertyName, string? value)
        {
            throw new NotImplementedException();
        }
    }
}