using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.RequiredTests
{
    public partial class Model
    {
        [Required]
        public string? RequiredString { get; set; }

        public string? OptionalString { get; set; }

        [Required]
        public int RequiredInt { get; set; }

        public int OptionalInt { get; set; }

        [Required]
        public DateTime RequiredDateTime { get; set; }

        public DateTime OptionalDateTime { get; set; }

        [Required]
        public SubModel? RequiredSubModel { get; set; }

        public SubModel? OptionalSubModel { get; set; }
    }

    public partial class SubModel
    {
        [Required]
        public string? SubRequiredString { get; set; }

        public string? SubOptionalString { get; set; }
    }
}
