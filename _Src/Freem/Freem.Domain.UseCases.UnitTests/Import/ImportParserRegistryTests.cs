using Freem.Domain.Formats;
using Freem.Domain.UseCases.Import;
using Freem.Domain.UseCases.UnitTests.Fixtures;

namespace Freem.Domain.UseCases.UnitTests.Import;

public sealed class ImportParserRegistryTests
{
    [Fact]
    public void Find_ShouldReturnParser_WhenKeyIsRegistered()
    {
        var parser = new FakeImportParser("first");
        var registry = new ImportParserRegistry([parser, new FakeImportParser("second")]);

        var result = registry.Find(FormatKey.Parse("first", null));

        Assert.True(result.Success);
        Assert.Same(parser, result.Value);
    }

    [Fact]
    public void Find_ShouldReturnFormatNotFound_WhenKeyIsUnknown()
    {
        var registry = new ImportParserRegistry([new FakeImportParser("first")]);

        var result = registry.Find(FormatKey.Parse("unknown", null));

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.FormatNotFound, result.Error.Type);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenKeysAreDuplicated()
    {
        var exception = Record.Exception(() => new ImportParserRegistry(
            [new FakeImportParser("same"), new FakeImportParser("same")]));

        Assert.IsType<ArgumentException>(exception);
    }
}
