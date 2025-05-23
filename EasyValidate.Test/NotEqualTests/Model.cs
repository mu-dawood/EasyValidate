using EasyValidate.Attributes;

namespace EasyValidate.Test.NotEqualTests
{
    public partial class Model
    {
        [NotEqualTo<string>("ForbiddenValue")]
        public string NotEqualString { get; set; } = string.Empty;

        [NotEqualTo<int>(0)]
        public int NotEqualInt { get; set; } = 0;

        [NotEqualTo<object>(null)]
        public object? NotEqualObject { get; set; }
    }
}
