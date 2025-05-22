using System;
using EasyValidate.Abstraction;
using Xunit;

namespace EasyValidate.Test.EqualToTests
{
    public class Tests
    {
        [Fact]
        public void EqualString_ShouldPass_WhenEqualToExpectedValue()
        {
            var model = new Model
            {
                EqualString = "ExpectedValue"
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void EqualString_ShouldFail_WhenNotEqualToExpectedValue()
        {
            var model = new Model
            {
                EqualString = "UnexpectedValue"
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("EqualString", result.Errors.Keys);
            Assert.Contains(result.Errors["EqualString"], e => e.ErrorCode == "EqualToValidationError");
        }

        [Fact]
        public void EqualInt_ShouldPass_WhenEqualTo42()
        {
            var model = new Model
            {
                EqualInt = 42
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void EqualInt_ShouldFail_WhenNotEqualTo42()
        {
            var model = new Model
            {
                EqualInt = 0
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("EqualInt", result.Errors.Keys);
            Assert.Contains(result.Errors["EqualInt"], e => e.ErrorCode == "EqualToValidationError");
        }

        [Fact]
        public void EqualObject_ShouldPass_WhenEqualToNull()
        {
            var model = new Model
            {
                EqualObject = null
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void EqualObject_ShouldFail_WhenNotEqualToNull()
        {
            var model = new Model
            {
                EqualObject = new object()
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("EqualObject", result.Errors.Keys);
            Assert.Contains(result.Errors["EqualObject"], e => e.ErrorCode == "EqualToValidationError");
        }

        [Fact]
        public void EqualDouble_ShouldPass_WhenEqualTo3Point14()
        {
            var model = new Model
            {
                EqualDouble = 3.14
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void EqualDouble_ShouldFail_WhenNotEqualTo3Point14()
        {
            var model = new Model
            {
                EqualDouble = 2.71
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("EqualDouble", result.Errors.Keys);
            Assert.Contains(result.Errors["EqualDouble"], e => e.ErrorCode == "EqualToValidationError");
        }

        [Fact]
        public void EqualBool_ShouldPass_WhenEqualToTrue()
        {
            var model = new Model
            {
                EqualBool = true
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void EqualBool_ShouldFail_WhenNotEqualToTrue()
        {
            var model = new Model
            {
                EqualBool = false
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("EqualBool", result.Errors.Keys);
            Assert.Contains(result.Errors["EqualBool"], e => e.ErrorCode == "EqualToValidationError");
        }
    }
}
