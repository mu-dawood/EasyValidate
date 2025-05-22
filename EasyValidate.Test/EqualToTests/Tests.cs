using System;
using EasyValidate.Abstraction;
using Xunit;

namespace EasyValidate.Test.EqualToTests
{
    public class Tests
    {
        [Fact]
        public void Validate_EqualString_ShouldPass()
        {
            var model = new Model
            {
                EqualString = "ExpectedValue"
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.EqualString)));
        }

        [Fact]
        public void Validate_EqualString_ShouldFail()
        {
            var model = new Model
            {
                EqualString = "UnexpectedValue"
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.EqualString)));
        }

        [Fact]
        public void Validate_EqualInt_ShouldPass()
        {
            var model = new Model
            {
                EqualInt = 42
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.EqualInt)));
        }

        [Fact]
        public void Validate_EqualInt_ShouldFail()
        {
            var model = new Model
            {
                EqualInt = 24
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.EqualInt)));
        }

        [Fact]
        public void Validate_EqualDouble_ShouldPass()
        {
            var model = new Model
            {
                EqualDouble = 3.14
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.EqualDouble)));
        }

        [Fact]
        public void Validate_EqualDouble_ShouldFail()
        {
            var model = new Model
            {
                EqualDouble = 2.71
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.EqualDouble)));
        }

        [Fact]
        public void Validate_EqualBool_ShouldPass()
        {
            var model = new Model
            {
                EqualBool = true
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.EqualBool)));
        }

        [Fact]
        public void Validate_EqualBool_ShouldFail()
        {
            var model = new Model
            {
                EqualBool = false
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.EqualBool)));
        }
    }
}
