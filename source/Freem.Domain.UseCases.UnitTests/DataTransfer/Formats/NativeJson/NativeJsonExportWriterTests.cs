using System.Text.Json;
using Freem.Domain.Entities.DataTransfer.Export;
using Freem.Domain.UseCases.Implementations.DataTransfer.Formats.NativeJson;
using Freem.Domain.UseCases.UnitTests.Fixtures;

namespace Freem.Domain.UseCases.UnitTests.DataTransfer.Formats.NativeJson;

public sealed class NativeJsonExportWriterTests
{
    private readonly NativeJsonExportWriter _writer = new();

    [Fact]
    public async Task WriteAsync_ShouldWriteVersionAndSections()
    {
        var items = ExportTestData.CreateItems();

        using var document = await WriteAsync(items);
        var root = document.RootElement;

        Assert.Equal("freem", root.GetProperty("format").GetString());
        Assert.Equal(1, root.GetProperty("version").GetInt32());
        Assert.Equal(2, root.GetProperty("tags").GetArrayLength());
        Assert.Equal(2, root.GetProperty("activities").GetArrayLength());
        Assert.Equal(2, root.GetProperty("records").GetArrayLength());
    }

    [Fact]
    public async Task WriteAsync_ShouldWriteRecordPeriodsAndOmitEmptyOptionalFields()
    {
        var items = ExportTestData.CreateItems();

        using var document = await WriteAsync(items);
        var records = document.RootElement.GetProperty("records");
        var full = records[0];
        var minimal = records[1];

        Assert.Equal("feature", full.GetProperty("name").GetString());
        Assert.Equal(2, full.GetProperty("periods").GetArrayLength());

        var firstPeriod = full.GetProperty("periods")[0];
        Assert.Equal("03:00:00", firstPeriod.GetProperty("time_zone_offset").GetString());
        Assert.Equal("morning", firstPeriod.GetProperty("description").GetString());

        Assert.False(minimal.TryGetProperty("name", out _));
        Assert.False(minimal.TryGetProperty("description", out _));
        Assert.False(minimal.GetProperty("periods")[0].TryGetProperty("description", out _));
    }

    [Fact]
    public async Task WriteAsync_ShouldWriteActivityStatusAndColor()
    {
        var items = ExportTestData.CreateItems();

        using var document = await WriteAsync(items);
        var activities = document.RootElement.GetProperty("activities");

        Assert.Equal("Active", activities[0].GetProperty("status").GetString());
        Assert.Equal("#aa00ff", activities[0].GetProperty("color").GetString());
        Assert.Equal("Archived", activities[1].GetProperty("status").GetString());
    }

    private async Task<JsonDocument> WriteAsync(IEnumerable<ExportItem> items)
    {
        using var stream = new MemoryStream();
        await _writer.WriteAsync(AsyncItems.From(items), stream);

        stream.Position = 0;
        return await JsonDocument.ParseAsync(stream);
    }
}
