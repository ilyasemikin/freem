using Freem.Entities.Storage.PostgreSQL.Database.Errors;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.Database.Errors;

public sealed class ErrorTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenCodeIsNull()
    {
        var exception = Record.Exception((() => new Error(null!, "Message")));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("code", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenMessageIsNull()
    {
        var exception = Record.Exception((() => new Error("Code", null!)));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("message", ((ArgumentNullException)exception).ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_ShouldThrowException_WhenCodeIsWhitespace(string code)
    {
        var exception = Record.Exception((() => new Error(code, "Message")));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("code", ((ArgumentException)exception).ParamName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_ShouldThrowException_WhenMessageIsWhitespace(string message)
    {
        var exception = Record.Exception((() => new Error("Code", message)));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("message", ((ArgumentException)exception).ParamName);
    }
    
    [Fact]
    public void TryParse_ShouldSuccess_WhenPassToStringResult()
    {
        var error = new Error(
            "Code", "Message", 
            new ErrorParameter("key1", "value1"),
            new ErrorParameter("key2", "value2"));
        
        var @string = error.ToString();
        
        var success = Error.TryParse(@string, out var result);
        
        Assert.True(success);
        Assert.Equal(error, result);
    }
}