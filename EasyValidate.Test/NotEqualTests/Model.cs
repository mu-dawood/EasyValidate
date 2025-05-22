using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.NotEqualTests
{
    public class Model
    {
        [NotEqualTo("ForbiddenValue")]
        public string? NotEqualString { get; set; }

        [NotEqualTo(0)]
        public int NotEqualInt { get; set; }

        [NotEqualTo(null)]
        public object? NotEqualObject { get; set; }
    }
}
