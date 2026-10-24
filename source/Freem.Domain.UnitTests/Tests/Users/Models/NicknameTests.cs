using Freem.Domain.Users.Models;

namespace Freem.Domain.UnitTests.Tests.Users.Models;

public sealed class NicknameTests
{
    [Theory]
    [InlineData("nickname")]
    public void Parse_ShouldKeepValue_WhenValueIsValid(string value)
    {
        var result = Nickname.Parse(value, null);
        var actual = result.ToString();

        Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => Nickname.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = Nickname.TryParse(value, null, out var result);

        Assert.False(success);
        Assert.Null(result);
    }
}
