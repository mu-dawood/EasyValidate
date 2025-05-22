using System;
using EasyValidate.Abstraction;
using Xunit;

namespace EasyValidate.Test.NotEqualTests
{
    public class Tests
    {
        [Fact]
        public void Validate_NotEqualString_ShouldPass()
        {
            var model = new Model
            {
                NotEqualString = "DifferentValue"
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.NotEqualString)));
        }

        [Fact]
        public void Validate_NotEqualString_ShouldFail()
        {
            var model = new Model
            {
                NotEqualString = "ForbiddenValue"
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.NotEqualString)));
        }

        [Fact]
        public void Validate_NotEqualInt_ShouldPass()
        {
            var model = new Model
            {
                NotEqualInt = 1
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.NotEqualInt)));
        }

        [Fact]
        public void Validate_NotEqualInt_ShouldFail()
        {
            var model = new Model
            {
                NotEqualInt = 0
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.NotEqualInt)));
        }

        [Fact]
        public void Validate_NotEqualObject_ShouldPass()
        {
            var forbiddenObject = new object();
            var model = new Model
            {
                NotEqualObject = new object()
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.NotEqualObject)));
        }

        [Fact]
        public void Validate_NotEqualObject_ShouldFail()
        {
            var model = new Model
            {
                NotEqualObject = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.NotEqualObject)));
        }
    }
}
