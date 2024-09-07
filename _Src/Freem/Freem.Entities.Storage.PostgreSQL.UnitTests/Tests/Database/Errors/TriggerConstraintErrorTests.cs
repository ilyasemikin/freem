using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.Database.Errors;

public sealed class TriggerConstraintErrorTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenCodeIsNull()
    {
        var exception = Record.Exception((() => new TriggerConstraintError(null!, "Message")));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("code", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenMessageIsNull()
    {
        var exception = Record.Exception((() => new TriggerConstraintError("Code", null!)));
        
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
        var exception = Record.Exception((() => new TriggerConstraintError(code, "Message")));
        
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
        var exception = Record.Exception((() => new TriggerConstraintError("Code", message)));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("message", ((ArgumentException)exception).ParamName);
    }

    [Fact]
    public void TryParse_ShouldSuccess_WhenPassInputWithoutParameters()
    {
        const string input = "[Code]: Message";
        var expected = new TriggerConstraintError("Code", "Message");
        
        var success = TriggerConstraintError.TryParse(input, out var result);
        
        Assert.True(success);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryParse_ShouldSuccess_WhenPassInputWithOneParameter()
    {
        const string input = "[Code] {key=value}: Message";
        var expected = new TriggerConstraintError(
            "Code", "Message", 
            new TriggerConstraintError.Parameter("key", "value"));

        var success = TriggerConstraintError.TryParse(input, out var result);
        
        Assert.True(success);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryParse_ShouldSuccess_WhenPassInputWithTwoParameters()
    {
        const string input = "[Code] {key1=value1;key2=value2}: Message";
        var expected = new TriggerConstraintError(
            "Code", "Message", 
            new TriggerConstraintError.Parameter("key1", "value1"),
            new TriggerConstraintError.Parameter("key2", "value2"));
        
        var success = TriggerConstraintError.TryParse(input, out var result);
        
        Assert.True(success);
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void TryParse_ShouldSuccess_WhenPassToStringResult()
    {
        var error = new TriggerConstraintError(
            "Code", "Message", 
            new TriggerConstraintError.Parameter("key1", "value1"),
            new TriggerConstraintError.Parameter("key2", "value2"));
        
        var @string = error.ToString();
        
        var success = TriggerConstraintError.TryParse(@string, out var result);
        
        Assert.True(success);
        Assert.Equal(error, result);
    }
}