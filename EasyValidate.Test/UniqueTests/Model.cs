using System.Collections.Generic;
using EasyValidate.Abstraction.Attributes;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Test.UniqueTests
{
    public class UniqueModel
    {
        [Unique("TestValue")]
        public string UniqueString { get; set; } = string.Empty;

        [Unique(42)]
        public int UniqueInt { get; set; } = 0;

        [Unique(3.14)]
        public double UniqueDouble { get; set; } = 0.0;

        public IEnumerable<string> StringCollection { get; set; } = new List<string>();
        public IEnumerable<int> IntCollection { get; set; } = new List<int>();
        public IEnumerable<double> DoubleCollection { get; set; } = new List<double>();

        public AttributeResult Validate()
        {
            var stringResult = new UniqueAttribute(UniqueString).Validate(nameof(UniqueString), StringCollection);
            if (!stringResult.IsValid)
            {
                return stringResult;
            }

            var intResult = new UniqueAttribute(UniqueInt).Validate(nameof(UniqueInt), IntCollection);
            if (!intResult.IsValid)
            {
                return intResult;
            }

            var doubleResult = new UniqueAttribute(UniqueDouble).Validate(nameof(UniqueDouble), DoubleCollection);
            if (!doubleResult.IsValid)
            {
                return doubleResult;
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
