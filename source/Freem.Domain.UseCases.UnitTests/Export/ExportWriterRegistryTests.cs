using Freem.Domain.Formats;
using Freem.Domain.UseCases.Implementations.Export;
using Freem.Domain.UseCases.UnitTests.Fixtures;

namespace Freem.Domain.UseCases.UnitTests.Export;

public sealed class ExportWriterRegistryTests
{
    [Fact]
    public void Find_ShouldReturnWriter_WhenKeyIsRegistered()
    {
        var writer = new FakeExportWriter("first");
        var registry = new ExportWriterRegistry([writer, new FakeExportWriter("second")]);

        var result = registry.Find(FormatKey.Parse("first", null));

        Assert.True(result.Success);
        Assert.Same(writer, result.Value);
    }

    [Fact]
    public void Find_ShouldReturnFormatNotFound_WhenKeyIsUnknown()
    {
        var registry = new ExportWriterRegistry([new FakeExportWriter("first")]);

        var result = registry.Find(FormatKey.Parse("unknown", null));

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.FormatNotFound, result.Error.Type);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenKeysAreDuplicated()
    {
        var exception = Record.Exception(() => new ExportWriterRegistry(
            [new FakeExportWriter("same"), new FakeExportWriter("same")]));

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Formats_ShouldBeOrderedByKey()
    {
        var registry = new ExportWriterRegistry([new FakeExportWriter("b"), new FakeExportWriter("a")]);

        var keys = registry.Formats.Select(format => format.Key.ToString()).ToArray();

        Assert.Equal(["a", "b"], keys);
    }
}
