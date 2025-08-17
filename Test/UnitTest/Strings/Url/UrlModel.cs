using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Url
{
    public partial class UrlModel
 : IValidate, IGenerate
    {
        [Url]
        public string Homepage { get; set; } = string.Empty;

        [Url]
        public string ApiEndpoint { get; set; } = string.Empty;

        [Optional, Url]
        public string? OptionalUrl { get; set; }
    }

    public partial class UrlNestedModel
 : IValidate, IGenerate
    {
        [Url]
        public string PrimaryUrl { get; set; } = string.Empty;

        public UrlModel? NestedUrls { get; set; }
    }
}
