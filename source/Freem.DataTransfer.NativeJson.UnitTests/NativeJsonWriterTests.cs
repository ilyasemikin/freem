using System.Text.Json;
using Freem.DataTransfer.NativeJson.Models;
using Freem.DataTransfer.NativeJson.UnitTests.Fixtures;

namespace Freem.DataTransfer.NativeJson.UnitTests;

public sealed class NativeJsonWriterTests
{
    private readonly NativeJsonWriter _writer = new();

    [Fact]
    public async Task WriteAsync_ShouldWriteVersionAndSections()
    {
        var items = NativeJsonTestData.CreateItems();

        using var document = await WriteAsync(items);
        var root = document.RootElement;

        Assert.Equal("freem", root.GetProperty("format").GetString());
        Assert.Equal(1, root.GetProperty("version").GetInt32());
        Assert.Equal(2, root.GetProperty("tags").GetArrayLength());
        Assert.Equal(2, root.GetProperty("activities").GetArrayLength());
        Assert.Equal(2, root.GetProperty("records").GetArrayLength());
    }

    [Fact]
    public async Task WriteAsync_ShouldWriteEmptySections_WhenThereAreNoItems()
    {
        using var document = await WriteAsync([]);
        var root = document.RootElement;

        Assert.Equal(0, root.GetProperty("tags").GetArrayLength());
        Assert.Equal(0, root.GetProperty("activities").GetArrayLength());
        Assert.Equal(0, root.GetProperty("records").GetArrayLength());
    }

    [Fact]
    public async Task WriteAsync_ShouldWriteEmptyMiddleSection_WhenThereAreNoActivities()
    {
        var items = NativeJsonTestData.CreateItems()
            .Where(item => item is not NativeJsonActivity)
            .ToArray();

        using var document = await WriteAsync(items);
        var root = document.RootElement;

        Assert.Equal(2, root.GetProperty("tags").GetArrayLength());
        Assert.Equal(0, root.GetProperty("activities").GetArrayLength());
        Assert.Equal(2, root.GetProperty("records").GetArrayLength());
    }

    [Fact]
    public async Task WriteAsync_ShouldWriteRecordPeriodsAndOmitNullProperties()
    {
        var items = NativeJsonTestData.CreateItems();

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
    public async Task WriteAsync_ShouldThrow_WhenItemsAreNotOrdered()
    {
        var items = NativeJsonTestData.CreateItems().Reverse().ToArray();
        using var stream = new MemoryStream();

        var exception = await Record.ExceptionAsync(
            () => _writer.WriteAsync(AsyncItems.From(items), stream));

        Assert.IsType<InvalidOperationException>(exception);
    }

    private async Task<JsonDocument> WriteAsync(IEnumerable<NativeJsonItem> items)
    {
        using var stream = new MemoryStream();
        await _writer.WriteAsync(AsyncItems.From(items), stream);

        stream.Position = 0;
        return await JsonDocument.ParseAsync(stream);
    }
}
