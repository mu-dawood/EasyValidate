using EasyValidate.Abstraction;
using EasyValidate.Abstraction.Attributes;
using Xunit;

namespace EasyValidate.Tests
{
    public class RangeAttributeTests
    {
        private class TestDto
        {
            [Range(1, 100)]
            public int Age { get; set; }
        }

        [Fact]
        public void Validate_RangeAttribute_ShouldPassForValidValue()
        {
            var dto = new TestDto { Age = 50 };
            var result = dto.Validate(null);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_RangeAttribute_ShouldFailForInvalidValue()
        {
            var dto = new TestDto { Age = 150 };
            var result = dto.Validate(null);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.Contains("Age"));
        }
    }
}
