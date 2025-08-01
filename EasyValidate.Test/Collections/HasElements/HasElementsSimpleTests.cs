using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Collections.HasElements;

public class HasElementsSimpleTests
{
    [Fact]
    public void ValidCollectionsWithElements_ShouldReturnValid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = new List<string> { "item1", "item2" },
            NumberEnumerable = new[] { 1, 2, 3 },
            StringArray = ["array1", "array2"],
            ObjectCollection = new List<object> { new(), "string", 123 },
            GuidHashSet = new HashSet<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void EmptyStringList_ShouldReturnInvalid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = [],
            NumberEnumerable = [1],
            StringArray = ["item"],
            ObjectCollection = [new()],
            GuidHashSet = [Guid.NewGuid()]
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("StringList"));
    }

    [Fact]
    public void NullStringList_ShouldReturnInvalid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = null,
            NumberEnumerable = [1],
            StringArray = ["item"],
            ObjectCollection = [new()],
            GuidHashSet = [Guid.NewGuid()]
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("StringList"));
    }

    [Fact]
    public void EmptyArray_ShouldReturnInvalid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = new List<string> { "item" },
            NumberEnumerable = new[] { 1 },
            StringArray = new string[0],
            ObjectCollection = new List<object> { new() },
            GuidHashSet = new HashSet<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("StringArray"));
    }

    [Fact]
    public void EmptyHashSet_ShouldReturnInvalid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = new List<string> { "item" },
            NumberEnumerable = new[] { 1 },
            StringArray = new[] { "item" },
            ObjectCollection = new List<object> { new() },
            GuidHashSet = new HashSet<Guid>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("GuidHashSet"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void StringList_WithVariousElementCounts_ShouldReturnValid(int elementCount)
    {
        // Arrange
        var items = Enumerable.Range(1, elementCount).Select(i => $"item{i}").ToList();
        var model = new HasElementsModel
        {
            StringList = items
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("StringList")));
    }

    [Fact]
    public void SingleElementCollections_ShouldReturnValid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = new List<string> { "single" },
            NumberEnumerable = new[] { 42 },
            StringArray = new[] { "only" },
            ObjectCollection = new List<object> { "one" },
            GuidHashSet = new HashSet<Guid> { Guid.NewGuid() }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void AllEmptyCollections_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = new List<string>(),
            NumberEnumerable = new int[0],
            StringArray = new string[0],
            ObjectCollection = new List<object>(),
            GuidHashSet = new HashSet<Guid>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Fact]
    public void AllNullCollections_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = null,
            NumberEnumerable = null,
            StringArray = null,
            ObjectCollection = null,
            GuidHashSet = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Fact]
    public void EnumerableWithYield_ShouldWork()
    {
        // Arrange
        IEnumerable<int> GenerateNumbers()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        var model = new HasElementsModel
        {
            NumberEnumerable = GenerateNumbers()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("NumberEnumerable")));
    }

    [Fact]
    public void CollectionWithNullElements_ShouldStillBeValid()
    {
        // Arrange
        var model = new HasElementsModel
        {
            StringList = new List<string> { null, "item", null },
            ObjectCollection = new List<object> { null, new() }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || (!result.GetAllErrors().Any(e => e.PropertyName.Contains("StringList")) && !result.GetAllErrors().Any(e => e.PropertyName.Contains("ObjectCollection"))));
    }

    [Fact]
    public void LargeCollection_ShouldReturnValid()
    {
        // Arrange
        var largeList = Enumerable.Range(1, 10000).Select(i => $"item{i}").ToList();
        var model = new HasElementsModel
        {
            StringList = largeList
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("StringList")));
    }
}
