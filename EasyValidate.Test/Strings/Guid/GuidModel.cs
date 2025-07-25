using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Guid
{
    public partial class GuidModel
 : IValidate
    {
        [Guid]
        public string UserId { get; set; } = string.Empty;

        [Guid]
        public string SessionId { get; set; } = string.Empty;

        [Optional, Guid]
        public string? OptionalGuid { get; set; }
    }

    public partial class GuidNestedModel
 : IValidate
    {
        [Guid]
        public string PrimaryGuid { get; set; } = string.Empty;

        public GuidModel? NestedGuids { get; set; }
    }
}
