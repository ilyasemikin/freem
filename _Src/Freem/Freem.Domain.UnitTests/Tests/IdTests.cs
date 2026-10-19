using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.UnitTests.Mocs.Entities;
using Freem.Domain.Users;

namespace Freem.Domain.UnitTests.Tests;

public sealed class IdTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrow_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => new TestId(value!));

        Assert.NotNull(exception);
    }

    public static TheoryData<Id, Id, bool> EqualsTestCases
    {
        get
        {
            var data = new TheoryData<Id, Id, bool>
            {
                { new TestId("123"), new OtherTestId("123"), false },
                { new TestId("123"), new OtherTestId("456"), false },
                { new TestId("123"), new TestId("123"), true },
                { new OtherTestId("456"), new OtherTestId("456"), true },
                { new TestId("123"), new TestId("456"), false }
            };

            var id = new TestId("123");
            data.Add(id, id, true);

            return data;
        }
    }

    [Theory]
    [MemberData(nameof(EqualsTestCases))]
    public void Equals_ShouldEqualsWithExpected(Id x, Id y, bool expected)
    {
        var result = x.Equals(y);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equals_ShouldFailure_WhenOtherIsNull()
    {
        var id = new TestId("123");

        var result = id.Equals(null);

        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(EqualsTestCases))]
    public void OperatorEquals_ShouldEqualWithExpected(Id x, Id y, bool expected)
    {
        var result = x == y;

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(EqualsTestCases))]
    public void OperatorNotEquals_ShouldNotEqualWithExpected(Id x, Id y, bool negativeExpected)
    {
        var result = x != y;

        Assert.Equal(!negativeExpected, result);
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_WhenIdsAreEqual()
    {
        var x = new TestId("123");
        var y = new TestId("123");

        Assert.Equal(x.GetHashCode(), y.GetHashCode());
    }

    [Fact]
    public void Generate_ShouldReturnDifferentIds_WhenCalledTwice()
    {
        var x = TagId.Generate();
        var y = TagId.Generate();

        Assert.NotEqual(x, y);
    }

    [Fact]
    public void Parse_ShouldKeepValue()
    {
        var id = TagId.Parse("123", null);
        var actual = id.ToString();

        Assert.Equal("123", actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldThrowFormatException_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => TagId.Parse(value!, null));

        Assert.IsType<FormatException>(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void TryParse_ShouldFailure_WhenValueIsInvalid(string? value)
    {
        var success = TagId.TryParse(value, null, out var id);

        Assert.False(success);
        Assert.Null(id);
    }

    [Fact]
    public void RunningRecordId_ShouldConvertFromAndToUserId()
    {
        var userId = UserId.Generate();

        var runningRecordId = RunningRecordId.FromUserId(userId);
        var actual = runningRecordId.ToUserId();
        var expectedValue = userId.ToString();
        var actualValue = runningRecordId.ToString();

        Assert.Equal(expectedValue, actualValue);
        Assert.Equal(userId, actual);
    }
}
