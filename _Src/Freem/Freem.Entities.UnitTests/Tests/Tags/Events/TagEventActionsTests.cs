using Freem.Entities.Tags.Events;
using Freem.Entities.UnitTests.Extensions;
using Freem.Reflection;

namespace Freem.Entities.UnitTests.Tests.Tags.Events;

public sealed class TagEventActionsTests
{
    public static TheoryData<string> ActivityActionsTestData =>
        typeof(TagEventActions).GetPublicConstantsStringValues();

    [Theory]
    [MemberData(nameof(ActivityActionsTestData))]
    public void All_ShouldContainsAll(string action)
    {
        var count = TagEventActions.All.Count(value => value == action);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void All_ShouldNotContainsExtra()
    {
        var expected = FieldsExtractor
            .GetPublicConstants(typeof(TagEventActions))
            .Count();

        var count = TagEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}