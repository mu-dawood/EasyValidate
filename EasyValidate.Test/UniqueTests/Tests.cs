using System.Collections.Generic;
using Xunit;

namespace EasyValidate.Test.UniqueTests
{
    public class UniqueTests
    {
        [Fact]
        public void Validate_UniqueString_ShouldPass()
        {
            var model = new UniqueModel
            {
                UniqueString = "TestValue",
                StringCollection = new List<string> { "Value1", "Value2", "Value3" }
            };

            var result = model.Validate();

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_UniqueString_ShouldFail()
        {
            var model = new UniqueModel
            {
                UniqueString = "TestValue",
                StringCollection = new List<string> { "TestValue", "Value2", "TestValue" }
            };

            var result = model.Validate();

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_UniqueInt_ShouldPass()
        {
            var model = new UniqueModel
            {
                UniqueInt = 42,
                IntCollection = new List<int> { 1, 2, 3 }
            };

            var result = model.Validate();

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_UniqueInt_ShouldFail()
        {
            var model = new UniqueModel
            {
                UniqueInt = 42,
                IntCollection = new List<int> { 42, 2, 42 }
            };

            var result = model.Validate();

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_UniqueDouble_ShouldPass()
        {
            var model = new UniqueModel
            {
                UniqueDouble = 3.14,
                DoubleCollection = new List<double> { 1.1, 2.2, 3.3 }
            };

            var result = model.Validate();

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_UniqueDouble_ShouldFail()
        {
            var model = new UniqueModel
            {
                UniqueDouble = 3.14,
                DoubleCollection = new List<double> { 3.14, 2.2, 3.14 }
            };

            var result = model.Validate();

            Assert.False(result.IsValid);
        }
    }
}
