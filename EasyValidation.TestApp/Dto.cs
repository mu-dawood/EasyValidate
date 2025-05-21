


using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.TestApp
{
    public partial class Dto
    {
        [Required]
        [NotNull]
        public string? Name { get; set; }

        // [Range(18, 99)]
        public int Age { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}