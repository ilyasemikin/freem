using Freem.Domain.Events;
using Freem.Domain.Tags;
using Utilities.Naming.Abstractions;

namespace Freem.Domain.UnitTests.Tests.Events;

public sealed class EventPayloadTests
{
    private static readonly INamingPolicy Policy = INamingPolicy.CamelCase;

    [Fact]
    public void Set_ShouldConvertNameByPolicy()
    {
        var payload = new EventPayload(Policy)
            .Set("TagName", "value");

        var dictionary = payload.ToReadOnlyDictionary();

        Assert.Equal("value", dictionary["tagName"]);
    }

    [Fact]
    public void Get_ShouldReturnParsedValue()
    {
        var id = TagId.Generate();
        var payload = new EventPayload(Policy)
            .Set("TagId", id);

        var result = payload.Get("TagId", TagId.Parse);

        Assert.Equal(id, result);
    }

    [Fact]
    public void GetRequired_ShouldThrowFormatException_WhenKeyIsMissing()
    {
        var payload = new EventPayload(Policy);

        var exception = Record.Exception(() => payload.GetRequired("Name"));

        Assert.IsType<FormatException>(exception);
    }

    [Fact]
    public void GetOptional_ShouldReturnNull_WhenValueIsNull()
    {
        var payload = new EventPayload(Policy)
            .Set<TagId>("TagId", null);

        var result = payload.GetOptional("TagId", TagId.Parse);

        Assert.Null(result);
    }

    [Fact]
    public void GetDate_ShouldReturnSameValue_AfterSetDate()
    {
        var date = new DateTimeOffset(2026, 10, 20, 12, 30, 15, TimeSpan.FromHours(3));
        var payload = new EventPayload(Policy)
            .SetDate("StartAt", date);

        var result = payload.GetDate("StartAt");

        Assert.Equal(date, result);
        Assert.Equal(date.Offset, result.Offset);
    }

    [Fact]
    public void GetList_ShouldReturnSameValues_AfterSetList()
    {
        var ids = new[] { TagId.Generate(), TagId.Generate() };
        var payload = new EventPayload(Policy)
            .SetList("TagIds", ids);

        var result = payload.GetList("TagIds", TagId.Parse);

        Assert.Equal(ids, result);
    }

    [Fact]
    public void GetList_ShouldReturnEmpty_AfterSetListWithoutValues()
    {
        var payload = new EventPayload(Policy)
            .SetList("TagIds", Array.Empty<TagId>());

        var result = payload.GetList("TagIds", TagId.Parse);

        Assert.Empty(result);
    }

    [Fact]
    public void Constructor_ShouldKeepDataLoadedFromStorage()
    {
        var data = new Dictionary<string, string?> { ["name"] = "value" };
        var payload = new EventPayload(data, Policy);

        var result = payload.GetRequired("Name");

        Assert.Equal("value", result);
    }
}
