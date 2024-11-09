using Freem.Entities.UnitTests.Extensions;
using Freem.Entities.Users.Events;
using Freem.Reflection;

namespace Freem.Entities.UnitTests.Tests.Users.Events;

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
        var expected = FieldsExtractor
            .GetPublicConstants(typeof(UserEventActions))
            .Count();

        var count = UserEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}