using Freem.Domain.Records.Events;
using Freem.Domain.UnitTests.Extensions;

namespace Freem.Domain.UnitTests.Tests.Records.Events;

public class RecordEventActionsTests
{
    public static TheoryData<string> ActivityActionsTestData =>
        typeof(RecordEventActions).GetPublicConstantsStringValues();

    [Theory]
    [MemberData(nameof(ActivityActionsTestData))]
    public void All_ShouldContainsAll(string action)
    {
        var count = RecordEventActions.All.Count(value => value == action);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void All_ShouldNotContainsExtra()
    {
        var expected = typeof(RecordEventActions)
            .GetPublicConstants()
            .Count();

        var count = RecordEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}