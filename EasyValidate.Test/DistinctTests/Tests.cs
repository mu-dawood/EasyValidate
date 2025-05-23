using System.Collections.Generic;
using Xunit;

namespace EasyValidate.Test.DistinctTests
{
    public class DistinctTests
    {
        [Fact]
        public void Validate_Distinct_ShouldPass()
        {
            var model = new DistinctModel
            {
                UniqueCollection = [1, 2, 3, 4]
            };
            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.UniqueCollection)));
        }

        [Fact]
        public void Validate_Duplicates_ShouldFail()
        {
            var model = new DistinctModel
            {
                UniqueCollection = [1, 2, 2, 4]
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.UniqueCollection)));
        }

        [Fact]
        public void Validate_EmptyCollection_ShouldPass()
        {
            var model = new DistinctModel
            {
                UniqueCollection = []
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.UniqueCollection)));
        }

        [Fact]
        public void Validate_NullCollection_ShouldThrowException()
        {
            var model = new DistinctModel
            {
                UniqueCollection = null
            };

            Assert.Throws<InvalidOperationException>(() => model.Validate());
        }
    }
}
