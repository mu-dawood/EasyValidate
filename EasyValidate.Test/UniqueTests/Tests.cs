using System.Collections.Generic;
using Xunit;

namespace EasyValidate.Test.UniqueTests
{
    public class UniqueTests
    {
        [Fact]
        public void Validate_UniqueStringCollection_ShouldPass()
        {
            var model = new UniqueModel
            {
                StringCollection = new List<string> { "Value1", "Value2", "Value3" }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Validate_UniqueStringCollection_ShouldFail()
        {
            var model = new UniqueModel
            {
                StringCollection = new List<string> { "Value1", "Value2", "Value1" }
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Key == nameof(model.StringCollection));
        }

        [Fact]
        public void Validate_UniqueIntCollection_ShouldPass()
        {
            var model = new UniqueModel
            {
                IntCollection = new List<int> { 1, 2, 3 }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Validate_UniqueIntCollection_ShouldFail()
        {
            var model = new UniqueModel
            {
                IntCollection = new List<int> { 1, 2, 1 }
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Key == nameof(model.IntCollection));
        }

        [Fact]
        public void Validate_UniqueDoubleCollection_ShouldPass()
        {
            var model = new UniqueModel
            {
                DoubleCollection = new List<double> { 1.1, 2.2, 3.3 }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Validate_UniqueDoubleCollection_ShouldFail()
        {
            var model = new UniqueModel
            {
                DoubleCollection = new List<double> { 1.1, 2.2, 1.1 }
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Key == nameof(model.DoubleCollection));
        }
    }
}
