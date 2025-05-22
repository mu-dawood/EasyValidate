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

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Validate_NotEqualString_ShouldFail()
        {
            var model = new Model
            {
                NotEqualString = "SameValue"
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Key == nameof(model.NotEqualString));
        }

        [Fact]
        public void Validate_NotEqualInt_ShouldPass()
        {
            var model = new Model
            {
                NotEqualInt = 1
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Validate_NotEqualInt_ShouldFail()
        {
            var model = new Model
            {
                NotEqualInt = 0
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Key == nameof(model.NotEqualInt));
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

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void Validate_NotEqualObject_ShouldFail()
        {
            var forbiddenObject = new object();
            var model = new Model
            {
                NotEqualObject = forbiddenObject
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Key == nameof(model.NotEqualObject));
        }
    }
}
