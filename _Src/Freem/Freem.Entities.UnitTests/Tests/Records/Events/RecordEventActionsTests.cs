using Freem.Entities.Records.Events;
using Freem.Entities.UnitTests.Extensions;
using Freem.Reflection;

namespace Freem.Entities.UnitTests.Tests.Records.Events;

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
        var expected = FieldsExtractor
            .GetPublicConstants(typeof(RecordEventActions))
            .Count();

        var count = RecordEventActions.All.Count;
        
        Assert.Equal(expected, count);
    }
}