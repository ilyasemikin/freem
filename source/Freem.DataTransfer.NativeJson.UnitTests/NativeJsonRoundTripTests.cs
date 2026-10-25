using Freem.DataTransfer.NativeJson.Models;
using Freem.DataTransfer.NativeJson.UnitTests.Fixtures;

namespace Freem.DataTransfer.NativeJson.UnitTests;

public sealed class NativeJsonRoundTripTests
{
    [Fact]
    public async Task WriteThenRead_ShouldKeepData()
    {
        var items = NativeJsonTestData.CreateItems();
        var tags = items.OfType<NativeJsonTag>().ToArray();
        var activities = items.OfType<NativeJsonActivity>().ToArray();
        var records = items.OfType<NativeJsonRecord>().ToArray();

        using var stream = new MemoryStream();
        await new NativeJsonWriter().WriteAsync(AsyncItems.From(items), stream);
        stream.Position = 0;
        var result = await new NativeJsonReader().ReadAsync(stream);

        Assert.True(result.Success);
        var file = result.Value;

        Assert.Equal(tags.Select(tag => (tag.Id, tag.Name)), file.Tags!.Select(tag => (tag.Id, tag.Name)));

        Assert.Equal(activities.Length, file.Activities!.Count);
        for (var i = 0; i < activities.Length; i++)
        {
            Assert.Equal(activities[i].Id, file.Activities[i].Id);
            Assert.Equal(activities[i].Name, file.Activities[i].Name);
            Assert.Equal(activities[i].Color, file.Activities[i].Color);
            Assert.Equal(activities[i].Status, file.Activities[i].Status);
            Assert.Equal(activities[i].TagIds!, file.Activities[i].TagIds!);
        }

        Assert.Equal(records.Length, file.Records!.Count);
        for (var i = 0; i < records.Length; i++)
        {
            var expected = records[i];
            var actual = file.Records[i];
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ActivityIds!, actual.ActivityIds!);
            Assert.Equal(expected.TagIds!, actual.TagIds!);

            Assert.Equal(expected.Periods!.Count, actual.Periods!.Count);
            for (var j = 0; j < expected.Periods.Count; j++)
            {
                Assert.Equal(expected.Periods[j].Id, actual.Periods[j].Id);
                Assert.Equal(expected.Periods[j].StartAt, actual.Periods[j].StartAt);
                Assert.Equal(expected.Periods[j].EndAt, actual.Periods[j].EndAt);
                Assert.Equal(expected.Periods[j].TimeZoneOffset, actual.Periods[j].TimeZoneOffset);
                Assert.Equal(expected.Periods[j].Description, actual.Periods[j].Description);
            }
        }
    }
}
