using Freem.Domain.UnitTests.Extensions;
using Freem.Domain.Users.Events;

namespace Freem.Domain.UnitTests.Tests.Users.Events;

public sealed class UserEventActionsTests
{
    public static TheoryData<string> ActivityActionsTestData =>
        typeof(UserEventActions).GetPublicConstantsStringValues();

    [Theory]
    [MemberData(nameof(ActivityActionsTestData))]
    public void All_ShouldContainsAll(string action)
    {
        var count = UserEventActions.All.Count(value => value == action);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void All_ShouldNotContainsExtra()
    {
        var expected = typeof(UserEventActions)
            .GetPublicConstants()
            .Count();

        var count = UserEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}