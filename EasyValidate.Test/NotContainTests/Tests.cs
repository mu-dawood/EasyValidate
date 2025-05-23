namespace EasyValidate.Test.NotContainTests
{
    public class NotContainTests
    {
        [Fact]
        public void Validate_Collection_ShouldPass()
        {
            var model = new TestModel { Values = [1, 3, 4] };
            var result = model.Validate();
            Assert.True(result.IsValid());
        }

        [Fact]
        public void Validate_Collection_ShouldFail()
        {
            var model = new TestModel { Values = [1, 2, 3] };
            var result = model.Validate();
            Assert.False(result.IsValid());
        }

        [Fact]
        public void Validate_String_ShouldPass()
        {
            var model = new TestModel { Text = "hello" };
            // 'z' is not in "hello"
            model.Text = "hello";
            var result = model.Validate();
            Assert.True(result.IsValid());
        }

        [Fact]
        public void Validate_String_ShouldFail()
        {
            var model = new TestModel { Text = "hello" };
            // 'e' is in "hello"
            var result = model.Validate();
            Assert.False(result.IsValid());
        }
    }
}
