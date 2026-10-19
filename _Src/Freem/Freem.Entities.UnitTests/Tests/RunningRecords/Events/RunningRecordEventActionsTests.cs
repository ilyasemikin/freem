using Freem.Entities.RunningRecords.Events;
using Freem.Entities.UnitTests.Extensions;

namespace Freem.Entities.UnitTests.Tests.RunningRecords.Events;

public sealed class RunningRecordEventActionsTests
{
    public static TheoryData<string> ActivityActionsTestData =>
        typeof(RunningRecordEventActions).GetPublicConstantsStringValues();

    [Theory]
    [MemberData(nameof(ActivityActionsTestData))]
    public void All_ShouldContainsAll(string action)
    {
        var count = RunningRecordEventActions.All.Count(value => value == action);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void All_ShouldNotContainsExtra()
    {
        var expected = typeof(RunningRecordEventActions)
            .GetPublicConstants()
            .Count();

        var count = RunningRecordEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}