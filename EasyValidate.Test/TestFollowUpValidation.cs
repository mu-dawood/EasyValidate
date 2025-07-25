using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.FollowUpValidation
{
    public partial class TestFollowUpValidation
 : IValidate
    {
        // This should trigger a warning for missing MinLength attribute
        [NotNull]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // This should trigger a warning for wrong sequence
        [MaxLength(100)]
        [NotNull]
        [MinLength(5)]
        public string Email { get; set; } = string.Empty;

        // This should be fine - correct sequence and all attributes present
        [NotNull]
        [MinLength(5)]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;
    }
}
