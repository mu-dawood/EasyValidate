using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.FollowUpValidation
{
    public partial class TestFollowUpValidation : IGenerate
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
