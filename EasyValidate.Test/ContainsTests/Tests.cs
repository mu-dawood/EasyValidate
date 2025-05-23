namespace EasyValidate.Test.ContainsTests
{
    public class ContainsTests
    {
        [Fact]
        public void Validate_CollectionContainsValue_ShouldPass()
        {
            var model = new ContainsModel
            {
                Items = [1, 2, 3, 4]
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.Items)));
        }

        [Fact]
        public void Validate_CollectionDoesNotContainValue_ShouldFail()
        {
            var model = new ContainsModel
            {
                Items = [5, 6, 7, 8]
            };

            var result = model.Validate();

            Assert.False(result.IsValid(nameof(model.Items)));
        }

        [Fact]
        public void Validate_StringContainsValue_ShouldPass()
        {
            var model = new ContainsStringModel
            {
                Text = "Hello, world!"
            };

            var result = model.Validate();

            Assert.True(result.IsValid(nameof(model.Text)));
        }

        [Fact]
        public void Validate_StringDoesNotContainValue_ShouldFail()
        {
            var model = new ContainsStringModel
            {
                Text = "Goodbye, world!"
            };

            var result = model.Validate();

            Assert.False(result.IsValid(nameof(model.Text)));
        }
    }
}
