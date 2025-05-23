using System.Collections.Generic;
using Xunit;
using EasyValidate.Test.MaxLengthTests;

namespace EasyValidate.Test.MaxLengthTests
{
    public class MaxLengthTests
    {
        [Fact]
        public void Validate_MaxLengthCollection_ShouldPass()
        {
            var model = new MaxLengthModel { MaxLengthCollection = new List<int> { 1, 2, 3 } };
            var result = model.Validate();
            Assert.True(result.IsValid());
        }

        [Fact]
        public void Validate_MaxLengthCollection_ShouldFail()
        {
            var model = new MaxLengthModel { MaxLengthCollection = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 } };
            var result = model.Validate();
            Assert.False(result.IsValid());
        }
    }
}
