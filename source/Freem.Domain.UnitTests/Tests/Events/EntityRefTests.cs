using Freem.Domain.Events;

namespace Freem.Domain.UnitTests.Tests.Events;

public sealed class EntityRefTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrow_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => new EntityRef(EntityType.Tag, value!));

        Assert.IsAssignableFrom<ArgumentException>(exception);
    }

    [Fact]
    public void Equals_ShouldBeTrue_WhenTypeAndValueAreEqual()
    {
        var first = new EntityRef(EntityType.Tag, "id");
        var second = new EntityRef(EntityType.Tag, "id");

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldBeFalse_WhenTypesAreDifferent()
    {
        var first = new EntityRef(EntityType.Tag, "id");
        var second = new EntityRef(EntityType.Activity, "id");

        Assert.NotEqual(first, second);
    }
}
