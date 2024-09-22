using Freem.Identifiers.Implementations.Generators;
using Freem.Identifiers.UnitTests.Mocks.Identifiers;

namespace Freem.Identifiers.UnitTests.Tests.Implementations.Generators;

public sealed class GuidStringIdentifierGeneratorTests
{
    [Fact]
    public void Generate_ShouldGenerateGuidStringAsValue()
    {
        var generator = new GuidStringIdentifierGenerator<FirstMockStringIdentifier>(value => new FirstMockStringIdentifier(value));

        var identifier = generator.Generate();
        
        Assert.NotNull(identifier);
        
        var @string = identifier.ToString();
        Assert.NotEmpty(@string);
        Assert.Equal(32, @string.Length);
    }
    
    [Fact]
    public void Generate_ShouldCallFactory_WhenCalled()
    {
        var called = false;
        var generator = new GuidStringIdentifierGenerator<FirstMockStringIdentifier>(value =>
        {
            called = true;
            return new FirstMockStringIdentifier(value);
        });
        
        generator.Generate();
        
        Assert.True(called);
    }

    [Fact]
    public void Generate_ShouldReturnDifferentIdentifiers_WhenCalledTwice()
    {
        var generator = new GuidStringIdentifierGenerator<FirstMockStringIdentifier>(value => new FirstMockStringIdentifier(value));
        
        var identifier1 = generator.Generate();
        var identifier2 = generator.Generate();
        
        Assert.NotEqual(identifier1, identifier2);
    }
}