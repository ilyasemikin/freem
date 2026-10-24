using Freem.Domain.Activities.Models;

namespace Freem.Domain.UnitTests.Tests.Activities.Models;

public sealed class ActivityStatusTests
{
    public static TheoryData<string, ActivityStatus> ValidTestCases { get; } = new()
    {
        { "Active", ActivityStatus.Active },
        { "Archived", ActivityStatus.Archived }
    };

    [Theory]
    [MemberData(nameof(ValidTestCases))]
    public void Parse_ShouldReturnStatus_WhenValueIsValid(string value, ActivityStatus expected)
    {
        var result = ActivityStatus.Parse(value, null);

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(ValidTestCases))]
    public void ToString_ShouldReturnParsableValue(string expected, ActivityStatus status)
    {
        var actual = status.ToString();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("active")]
    [InlineData("Unknown")]
    [InlineData("5")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = ActivityStatus.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid()
    {
        var exception = Record.Exception(() => ActivityStatus.Parse("Unknown", null));

        Assert.IsType<FormatException>(exception);
    }
}
