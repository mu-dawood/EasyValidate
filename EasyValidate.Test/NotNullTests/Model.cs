using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.NotNullTests
{
    public partial class Model
    {
        [NotNull]
        public string? NotNullString { get; set; }

        [NotNull]
        public object? NotNullObject { get; set; }

        [NotNull]
        public int? NotNullNullableInt { get; set; } = 0;
    }
}
