


using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.TestApp
{
    public partial class Dto
    {
        [Required]
        [Range(18, 99)]
        public string? Name { get; set; }

        [Range(18, 99)]
        public byte Age { get; set; }

        public string? Email { get; set; }
    }
}