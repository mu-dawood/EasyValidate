using System;
using EasyValidate.Abstraction;
using Xunit;

namespace EasyValidate.Test.NotEqualTests
{
    public class Tests
    {
        [Fact]
        public void NotEqualString_ShouldFail_WhenEqualToForbiddenValue()
        {
            var model = new Model
            {
                NotEqualString = "ForbiddenValue"
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("NotEqualString", result.Errors.Keys);
            Assert.Contains(result.Errors["NotEqualString"], e => e.ErrorCode == "NotEqualToValidationError");
        }

        [Fact]
        public void NotEqualString_ShouldPass_WhenNotEqualToForbiddenValue()
        {
            var model = new Model
            {
                NotEqualString = "AllowedValue"
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void NotEqualInt_ShouldFail_WhenEqualToZero()
        {
            var model = new Model
            {
                NotEqualInt = 0
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("NotEqualInt", result.Errors.Keys);
            Assert.Contains(result.Errors["NotEqualInt"], e => e.ErrorCode == "NotEqualToValidationError");
        }

        [Fact]
        public void NotEqualInt_ShouldPass_WhenNotEqualToZero()
        {
            var model = new Model
            {
                NotEqualInt = 1
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void NotEqualObject_ShouldFail_WhenEqualToNull()
        {
            var model = new Model
            {
                NotEqualObject = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("NotEqualObject", result.Errors.Keys);
            Assert.Contains(result.Errors["NotEqualObject"], e => e.ErrorCode == "NotEqualToValidationError");
        }

        [Fact]
        public void NotEqualObject_ShouldPass_WhenNotEqualToNull()
        {
            var model = new Model
            {
                NotEqualObject = new object()
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }
    }
}
