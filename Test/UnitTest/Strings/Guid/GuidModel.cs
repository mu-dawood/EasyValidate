using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Guid
{
    public partial class GuidModel
 : IValidate, IGenerate
    {
        [Guid]
        public string UserId { get; set; } = string.Empty;

        [Guid]
        public string SessionId { get; set; } = string.Empty;

        [Optional, Guid]
        public string? OptionalGuid { get; set; }
    }

    public partial class GuidNestedModel
 : IValidate, IGenerate
    {
        [Guid]
        public string PrimaryGuid { get; set; } = string.Empty;

        public GuidModel? NestedGuids { get; set; }
    }
}
