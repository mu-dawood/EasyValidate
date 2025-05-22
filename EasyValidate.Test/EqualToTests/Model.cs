using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.EqualToTests
{
    public class Model
    {
        [EqualTo("ExpectedValue")]
        public string? EqualString { get; set; }

        [EqualTo(42)]
        public int EqualInt { get; set; }

        [EqualTo(null)]
        public object? EqualObject { get; set; }

        [EqualTo(3.14)]
        public double EqualDouble { get; set; }

        [EqualTo(true)]
        public bool EqualBool { get; set; }
    }
}
