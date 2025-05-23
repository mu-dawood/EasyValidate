namespace EasyValidate.Test.NotNullTests
{
    public class Tests
    {
        [Fact]
        public void Validate_NotNullString_ShouldPass()
        {
            var model = new Model
            {
                NotNullString = "NonNullValue"
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.NotNullString)));
        }

        [Fact]
        public void Validate_NotNullString_ShouldFail()
        {
            var model = new Model
            {
                NotNullString = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.NotNullString)));
        }

        [Fact]
        public void Validate_NotNullObject_ShouldPass()
        {
            var model = new Model
            {
                NotNullObject = new object()
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.NotNullObject)));
        }

        [Fact]
        public void Validate_NotNullObject_ShouldFail()
        {
            var model = new Model
            {
                NotNullObject = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.NotNullObject)));
        }

        [Fact]
        public void Validate_NotNullNullableInt_ShouldPass()
        {
            var model = new Model
            {
                NotNullNullableInt = 1
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.NotNullNullableInt)));
        }

        [Fact]
        public void Validate_NotNullNullableInt_ShouldFail()
        {
            var model = new Model
            {
                NotNullNullableInt = null
            };

            var result = model.Validate();

            Assert.True(result.HasErrors(nameof(model.NotNullNullableInt)));
        }
    }
}
