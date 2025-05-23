namespace EasyValidate.Test.LengthTests
{
    public class LengthTests
    {
        [Fact]
        public void Validate_FixedLengthCollection_ShouldPass()
        {
            var model = new LengthModel { FixedLengthCollection = new List<int> { 1, 2, 3, 4, 5 } };
            var result = model.Validate();
            Assert.True(result.IsValid());
        }

        [Fact]
        public void Validate_FixedLengthCollection_ShouldFail()
        {
            var model = new LengthModel { FixedLengthCollection = new List<int> { 1, 2, 3 } };
            var result = model.Validate();
            Assert.False(result.IsValid());
        }
    }
}
