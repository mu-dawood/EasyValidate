using Xunit;

namespace EasyValidate.Test.SingleTests
{
    public class SingleTests
    {
        [Fact]
        public void Validate_SingleValue_ShouldPass()
        {
            var model = new SingleModel
            {
                Collection = new[] { 1, 2, 3 }
            };
            var result = model.Validate();
            Assert.True(result.IsValid(nameof(model.Collection)));
        }

        [Fact]
        public void Validate_MultipleOccurrences_ShouldFail()
        {
            var model = new SingleModel
            {
                Collection = new[] { 2, 2, 3 }
            };
            var result = model.Validate();
            Assert.True(result.HasErrors(nameof(model.Collection)));
        }

        [Fact]
        public void Validate_NoOccurrence_ShouldFail()
        {
            var model = new SingleModel
            {
                Collection = new[] { 1, 3, 4 }
            };
            var result = model.Validate();
            Assert.True(result.HasErrors(nameof(model.Collection)));
        }

        [Fact]
        public void Validate_EmptyCollection_ShouldFail()
        {
            var model = new SingleModel
            {
                Collection = new int[] { }
            };
            var result = model.Validate();
            Assert.True(result.HasErrors(nameof(model.Collection)));
        }

        [Fact]
        public void Validate_NullCollection_ShouldThrow()
        {
            var model = new SingleModel
            {
                Collection = null
            };
            Assert.Throws<ArgumentNullException>(() => model.Validate());
        }
    }
}
