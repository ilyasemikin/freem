using Freem.Entities.Activities.Events;
using Freem.Entities.UnitTests.Extensions;
using Freem.Reflection;

namespace Freem.Entities.UnitTests.Tests.Activities.Events;

public sealed class ActivityEventActionsTests
{
    public static TheoryData<string> ActivityActionsTestData =>
        typeof(ActivityEventActions).GetPublicConstantsStringValues();

    [Theory]
    [MemberData(nameof(ActivityActionsTestData))]
    public void All_ShouldContainsAll(string action)
    {
        var count = ActivityEventActions.All.Count(value => value == action);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void All_ShouldNotContainsExtra()
    {
        var expected = FieldsExtractor
            .GetPublicConstants(typeof(ActivityEventActions))
            .Count();

        var count = ActivityEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}