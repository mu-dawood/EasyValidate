namespace EasyValidate.Test.MinLengthTests
{
    public class MinLengthTests
    {
        [Fact]
        public void Validate_MinLengthCollection_ShouldPass()
        {
            var model = new MinLengthModel { MinLengthCollection = new List<int> { 1, 2, 3, 4 } };
            var result = model.Validate();
            Assert.True(result.IsValid());
        }

        [Fact]
        public void Validate_MinLengthCollection_ShouldFail()
        {
            var model = new MinLengthModel { MinLengthCollection = new List<int> { 1 } };
            var result = model.Validate();
            Assert.False(result.IsValid());
        }
    }
}
