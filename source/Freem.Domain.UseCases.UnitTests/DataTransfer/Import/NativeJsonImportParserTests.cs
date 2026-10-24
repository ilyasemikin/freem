using System.Text;
using Freem.Domain.UseCases.Implementations.DataTransfer.Import;
using Freem.Domain.Users.Models;

namespace Freem.Domain.UseCases.UnitTests.DataTransfer.Import;

public sealed class NativeJsonImportParserTests
{
    private readonly NativeJsonImportParser _parser = new();
    private static readonly ImportParseOptions DefaultOptions = new(TimeZoneOffset.Default);

    [Fact]
    public async Task ParseAsync_ShouldReturnDocument_WhenFileIsValid()
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

        var result = await ParseAsync(json);

        Assert.True(result.Success);
        var document = result.Value;
        Assert.Single(document.Tags);
        Assert.Equal("t1", document.Activities[0].TagIds[0].ToString());
        Assert.Equal("a1", document.Records[0].ActivityIds[0].ToString());
        Assert.Equal(TimeSpan.FromHours(1), document.Records[0].Periods[0].Period.Duration);
        Assert.Equal(TimeSpan.FromHours(3), document.Records[0].Periods[0].TimeZoneOffset.Value);
    }

    [Theory]
    [InlineData("not a json")]
    [InlineData("null")]
    [InlineData("""{ "format": "other", "version": 1 }""")]
    [InlineData("""{ "format": "freem", "version": 2 }""")]
    [InlineData("""{ "format": "freem" }""")]
    [InlineData("""{ "format": "freem", "version": 1, "tags": [{ "id": "t1" }] }""")]
    [InlineData("""{ "format": "freem", "version": 1, "tags": [{ "id": "t1", "name": "a" }, { "id": "t1", "name": "b" }] }""")]
    [InlineData("""{ "format": "freem", "version": 1, "activities": [{ "id": "a1", "name": "x", "status": "Unknown" }] }""")]
    [InlineData("""{ "format": "freem", "version": 1, "activities": [{ "id": "a1", "name": "x", "status": "Active", "tag_ids": ["missing"] }] }""")]
    [InlineData("""{ "format": "freem", "version": 1, "records": [{ "id": "r1", "periods": [] }] }""")]
    public async Task ParseAsync_ShouldReturnInvalidRequest_WhenFileIsInvalid(string json)
    {
        var result = await ParseAsync(json);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.InvalidRequest, result.Error.Type);
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnInvalidRequest_WhenPeriodHasNonPositiveDuration()
    {
        const string json = """
            {
              "format": "freem",
              "version": 1,
              "activities": [{ "id": "a1", "name": "coding", "status": "Active" }],
              "records": [{
                "id": "r1",
                "activity_ids": ["a1"],
                "periods": [{
                  "id": "p1",
                  "start_at": "2026-10-20T10:00:00+00:00",
                  "end_at": "2026-10-20T10:00:00+00:00",
                  "time_zone_offset": "00:00:00"
                }]
              }]
            }
            """;

        var result = await ParseAsync(json);

        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.InvalidRequest, result.Error.Type);
    }

    private async Task<Utilities.Primitives.Result<Freem.Domain.Entities.DataTransfer.Import.ImportParseResult, Utilities.Errors.Error>> ParseAsync(
        string json)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        return await _parser.ParseAsync(stream, DefaultOptions);
    }
}
