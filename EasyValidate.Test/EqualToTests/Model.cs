using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.EqualToTests
{
    public partial class Model
    {
        [EqualTo<string>("ExpectedValue")]
        // [Range(0, 100)]
        public string EqualString { get; set; } = string.Empty;

        [EqualTo<int>(42)]
        public int EqualInt { get; set; } = 0;

        [EqualTo<object>(null)]
        public object? EqualObject { get; set; }

        [EqualTo<double>(3.14)]
        public double EqualDouble { get; set; } = 0.0;

        [EqualTo<bool>(true)]
        public bool EqualBool { get; set; } = false;
    }
}
