using Freem.Identifiers.Abstractions;
using Freem.Identifiers.Base;
using Freem.Identifiers.UnitTests.Mocks.Identifiers;

namespace Freem.Identifiers.UnitTests.Tests.Base;

public class StringIdentifierTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrow_WhenValueIsInvalid(string? value)
    {
        var exception = Record.Exception(() => new FirstMockStringIdentifier(value!));
        
        Assert.NotNull(exception);
    }

    public static TheoryData<StringIdentifier, StringIdentifier?, bool> NullableEqualsTestCases { get; } =
        new()
        {
            { new FirstMockStringIdentifier("123"), null, false }
        };
    
    public static TheoryData<StringIdentifier, StringIdentifier, bool> EqualsTestCases { get; } =
        new()
        {
            { new FirstMockStringIdentifier("123"), new SecondMockStringIdentifier("123"), false },
            { new FirstMockStringIdentifier("123"), new SecondMockStringIdentifier("456"), false },
            { new FirstMockStringIdentifier("123"), new FirstMockStringIdentifier("123"), true },
            { new SecondMockStringIdentifier("456"), new SecondMockStringIdentifier("456"), true }
        };

    [Theory]
    [MemberData(nameof(NullableEqualsTestCases))]
    [MemberData(nameof(EqualsTestCases))]
    public void Equals_ShouldEqualsWithExpected(StringIdentifier x, StringIdentifier? y, bool expected)
    {
        var result = x.Equals(y);
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(EqualsTestCases))]
    public void OperatorEquals_ShouldEqualWithExpected(StringIdentifier x, StringIdentifier y, bool expected)
    {
        var result = x == y;
        
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(EqualsTestCases))]
    public void OperatorNotEquals_ShouldNotEqualWithExpected(StringIdentifier x, StringIdentifier y, bool negativeExpected)
    {
        var result = x != y;
        
        Assert.Equal(!negativeExpected, result);
    }
}