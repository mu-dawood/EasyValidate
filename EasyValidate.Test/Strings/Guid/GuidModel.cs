using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Guid
{
    public partial class GuidModel
    {
        [Guid]
        public string UserId { get; set; } = string.Empty;

        [Guid]
        public string SessionId { get; set; } = string.Empty;

        [Optional, Guid]
        public string? OptionalGuid { get; set; }
    }

    public partial class GuidNestedModel
    {
        [Guid]
        public string PrimaryGuid { get; set; } = string.Empty;

        public GuidModel? NestedGuids { get; set; }
    }
}
