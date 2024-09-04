using Freem.Collections.Extensions;

namespace Freem.Collections.UnitTests.Tests;

public sealed class EnumerableExtensionsTests
{
    public static TheoryData<IEnumerable<int>, IEnumerable<int>, bool> UnorderedEqualsSuccessCases
    {
        get
        {
            var data = new TheoryData<IEnumerable<int>, IEnumerable<int>, bool>();
            
            data.Add([1, 2, 3], [1, 2, 3], true);
            data.Add([1, 2, 3], [2, 3, 1], true);
            data.Add([1, 2, 3], [3, 2, 1], true);
            data.Add([1, 2, 2, 3], [1, 2, 2, 3], true);
            data.Add([1, 2, 2, 3], [2, 1, 3, 2], true);
            
            data.Add([1, 2, 3], [1, 2, 4], false);
            data.Add([1, 2, 4], [1, 2], false);
            data.Add([1, 2, 3], [4, 5, 6], false);

            return data;
        }
    }
    
    [Theory]
    [MemberData(nameof(UnorderedEqualsSuccessCases))]
    public void UnorderedEquals_ShouldSuccess_WhenPassValid(
        IEnumerable<int> first, 
        IEnumerable<int> second, 
        bool expected)
    {
        var success = first.UnorderedEquals(second);
        
        Assert.Equal(success, expected);
    }

    [Fact]
    public void UnorderedEquals_ShouldThrowException_WhenFirstNull()
    {
        var exception = Record.Exception(() => EnumerableExtensions.UnorderedEquals<int>(null!, []));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("first", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void UnorderedEquals_ShouldThrowException_WhenSecondNull()
    {
        var exception = Record.Exception(() => EnumerableExtensions.UnorderedEquals<int>([], null!));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("second", ((ArgumentNullException)exception).ParamName);
    }
}