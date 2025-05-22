using System.Collections.Generic;
using Xunit;

namespace EasyValidate.Test.NoDuplicatesTests
{
    public class NoDuplicatesTests
    {
        [Fact]
        public void Validate_NoDuplicates_ShouldPass()
        {
            var model = new NoDuplicatesModel
            {
                UniqueCollection = [1, 2, 3, 4]
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.UniqueCollection)));
        }

        [Fact]
        public void Validate_Duplicates_ShouldFail()
        {
            var model = new NoDuplicatesModel
            {
                UniqueCollection = [1, 2, 2, 4]
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.UniqueCollection)));
        }

        [Fact]
        public void Validate_EmptyCollection_ShouldPass()
        {
            var model = new NoDuplicatesModel
            {
                UniqueCollection = []
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.UniqueCollection)));
        }

        [Fact]
        public void Validate_NullCollection_ShouldThrowException()
        {
            var model = new NoDuplicatesModel
            {
                UniqueCollection = null
            };

            Assert.Throws<InvalidOperationException>(() => model.Validate());
        }
    }
}
