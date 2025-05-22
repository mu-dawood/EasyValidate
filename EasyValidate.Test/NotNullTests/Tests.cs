using System;
using EasyValidate.Abstraction;
using Xunit;

namespace EasyValidate.Test.NotNullTests
{
    public class Tests
    {
        [Fact]
        public void NotNullString_ShouldFail_WhenNull()
        {
            var model = new Model
            {
                NotNullString = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("NotNullString", result.Errors.Keys);
            Assert.Contains(result.Errors["NotNullString"], e => e.ErrorCode == "NotNullValidationError");
        }

        [Fact]
        public void NotNullString_ShouldPass_WhenNotNull()
        {
            var model = new Model
            {
                NotNullString = "Valid"
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void NotNullObject_ShouldFail_WhenNull()
        {
            var model = new Model
            {
                NotNullObject = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("NotNullObject", result.Errors.Keys);
            Assert.Contains(result.Errors["NotNullObject"], e => e.ErrorCode == "NotNullValidationError");
        }

        [Fact]
        public void NotNullObject_ShouldPass_WhenNotNull()
        {
            var model = new Model
            {
                NotNullObject = new object()
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void NotNullNullableInt_ShouldFail_WhenNull()
        {
            var model = new Model
            {
                NotNullNullableInt = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("NotNullNullableInt", result.Errors.Keys);
            Assert.Contains(result.Errors["NotNullNullableInt"], e => e.ErrorCode == "NotNullValidationError");
        }

        [Fact]
        public void NotNullNullableInt_ShouldPass_WhenNotNull()
        {
            var model = new Model
            {
                NotNullNullableInt = 42
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }
    }
}
