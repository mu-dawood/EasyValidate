using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EasyValidate.Abstraction;
using Xunit;

namespace EasyValidate.Test.RequiredTests
{
    public class Tests
    {
        [Fact]
        public void RequiredString_ShouldFail_WhenNull()
        {
            var model = new Model
            {
                RequiredString = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("RequiredString", result.Errors.Keys);
            Assert.Contains(result.Errors["RequiredString"], e => e.ErrorCode == "RequiredValidationError");
        }

        [Fact]
        public void RequiredString_ShouldPass_WhenNotNull()
        {
            var model = new Model
            {
                RequiredString = "Valid",
                RequiredInt = 1,
                RequiredDateTime = DateTime.Now,
                RequiredSubModel = new SubModel
                {
                    SubRequiredString = "Valid"
                }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void RequiredString_ShouldPass_WhenEmpty()
        {
            var model = new Model
            {
                RequiredString = "",
                RequiredInt = 1,
                RequiredDateTime = DateTime.Now,
                RequiredSubModel = new SubModel
                {
                    SubRequiredString = "Valid"
                }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void RequiredSubModel_ShouldFail_WhenNull()
        {
            var model = new Model
            {
                RequiredSubModel = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("RequiredSubModel", result.Errors.Keys);
            Assert.Contains(result.Errors["RequiredSubModel"], e => e.ErrorCode == "RequiredValidationError");
        }

        [Fact]
        public void RequiredSubModel_ShouldPass_WhenValid()
        {
            var model = new Model
            {
                RequiredString = "Valid",
                RequiredInt = 1,
                RequiredDateTime = DateTime.Now,
                RequiredSubModel = new SubModel
                {
                    SubRequiredString = "Valid"
                }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }


        [Fact]
        public void SubRequiredString_ShouldFail_WhenNull()
        {
            var model = new Model
            {
                RequiredSubModel = new SubModel
                {
                    SubRequiredString = null
                }
            };

            var result = model.Validate();

            Assert.True(result.HasErrors);
            Assert.Contains("RequiredSubModel.SubRequiredString", result.Errors.Keys);
            Assert.Contains(result.Errors["RequiredSubModel.SubRequiredString"], e => e.ErrorCode == "RequiredValidationError");
        }

        [Fact]
        public void RequiredInt_ShouldPass_WhenDefault()
        {
            var model = new Model
            {
                RequiredString = "Valid",
                RequiredInt = 0,
                RequiredDateTime = DateTime.Now,
                RequiredSubModel = new SubModel
                {
                    SubRequiredString = "Valid"
                }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }

        [Fact]
        public void RequiredDateTime_ShouldPass_WhenDefault()
        {
            var model = new Model
            {
                RequiredString = "Valid",
                RequiredInt = 1,
                RequiredDateTime = default,
                RequiredSubModel = new SubModel
                {
                    SubRequiredString = "Valid"
                }
            };

            var result = model.Validate();

            Assert.False(result.HasErrors);
        }
    }
}
