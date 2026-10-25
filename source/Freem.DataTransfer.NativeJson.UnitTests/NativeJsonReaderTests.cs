using System.Text;
using Freem.DataTransfer.NativeJson.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.DataTransfer.NativeJson.UnitTests;

public sealed class NativeJsonReaderTests
{
    private readonly NativeJsonReader _reader = new();

    [Fact]
    public async Task ReadAsync_ShouldReturnFile_WhenFileIsValid()
    {
        const string json = """
            {
              "format": "freem",
              "version": 1,
              "tags": [{ "id": "t1", "name": "deep" }],
              "activities": [{ "id": "a1", "name": "coding", "color": "#aa00ff", "status": "Active", "tag_ids": ["t1"] }],
              "records": [{
                "id": "r1",
                "name": "feature",
                "activity_ids": ["a1"],
                "tag_ids": ["t1"],
                "periods": [{
                  "id": "p1",
                  "start_at": "2026-10-20T10:00:00+00:00",
                  "end_at": "2026-10-20T11:00:00+00:00",
                  "time_zone_offset": "03:00:00"
                }]
              }]
            }
            """;

        var result = await ReadAsync(json);

        Assert.True(result.Success);
        var file = result.Value;
        Assert.Equal("deep", file.Tags![0].Name);
        Assert.Equal(["t1"], file.Activities![0].TagIds!);
        Assert.Equal(["a1"], file.Records![0].ActivityIds!);
        var period = file.Records[0].Periods![0];
        Assert.Equal(TimeSpan.FromHours(1), period.EndAt - period.StartAt);
        Assert.Equal("03:00:00", period.TimeZoneOffset);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnFileWithoutSections_WhenSectionsAreMissing()
    {
        var result = await ReadAsync("""{ "format": "freem", "version": 1 }""");

        Assert.True(result.Success);
        Assert.Null(result.Value.Tags);
        Assert.Null(result.Value.Activities);
        Assert.Null(result.Value.Records);
    }

    public static TheoryData<string, ErrorType> InvalidFiles => new()
    {
        { "not a json", NativeJsonErrorCatalog.Types.InvalidJson },
        { "null", NativeJsonErrorCatalog.Types.EmptyFile },
        { """{ "format": "other", "version": 1 }""", NativeJsonErrorCatalog.Types.UnexpectedFormat },
        { """{ "version": 1 }""", NativeJsonErrorCatalog.Types.UnexpectedFormat },
        { """{ "format": "freem", "version": 2 }""", NativeJsonErrorCatalog.Types.UnsupportedVersion },
        { """{ "format": "freem" }""", NativeJsonErrorCatalog.Types.UnsupportedVersion }
    };

    [Theory]
    [MemberData(nameof(InvalidFiles))]
    public async Task ReadAsync_ShouldReturnError_WhenFileIsInvalid(string json, ErrorType expected)
    {
        var result = await ReadAsync(json);

        Assert.False(result.Success);
        Assert.Equal(expected, result.Error.Type);
    }

    private async Task<Result<NativeJsonFile, Error>> ReadAsync(string json)
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(bytes);
        return await _reader.ReadAsync(stream);
    }
}
