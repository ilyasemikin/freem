using Freem.Domain.Activities.Events;
using Freem.Domain.UnitTests.Extensions;

namespace Freem.Domain.UnitTests.Tests.Activities.Events;

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
        var expected = typeof(ActivityEventActions)
            .GetPublicConstants()
            .Count();

        var count = ActivityEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}