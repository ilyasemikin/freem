using Freem.Domain.Entities.Export;
using Freem.Domain.UseCases.Export;
using Freem.Domain.UseCases.Import;
using Freem.Domain.UseCases.UnitTests.Fixtures;

namespace Freem.Domain.UseCases.UnitTests.Formats;

public sealed class NativeJsonRoundTripTests
{
    [Fact]
    public async Task ExportThenImport_ShouldKeepData()
    {
        var items = ExportTestData.CreateItems();
        var tags = items.OfType<ExportTag>().ToArray();
        var activities = items.OfType<ExportActivity>().ToArray();
        var records = items.OfType<ExportRecord>().ToArray();

        using var stream = new MemoryStream();
        await new NativeJsonExportWriter().WriteAsync(AsyncItems.From(items), stream);
        stream.Position = 0;
        var result = await new NativeJsonImportParser().ParseAsync(stream);

        Assert.True(result.Success);
        var document = result.Value;

        Assert.Equal(tags.Select(t => t.Id.ToString()), document.Tags.Select(t => t.Id.ToString()));
        Assert.Equal(tags.Select(t => t.Name), document.Tags.Select(t => t.Name));

        Assert.Equal(activities.Length, document.Activities.Count);
        for (var i = 0; i < activities.Length; i++)
        {
            Assert.Equal(activities[i].Id.ToString(), document.Activities[i].Id.ToString());
            Assert.Equal(activities[i].Name, document.Activities[i].Name);
            Assert.Equal(activities[i].Color, document.Activities[i].Color);
            Assert.Equal(activities[i].Status, document.Activities[i].Status);
            Assert.Equal(activities[i].TagIds.Select(id => id.ToString()), document.Activities[i].TagIds.Select(k => k.ToString()));
        }

        Assert.Equal(records.Length, document.Records.Count);
        for (var i = 0; i < records.Length; i++)
        {
            var expected = records[i];
            var actual = document.Records[i];
            Assert.Equal(expected.Id.ToString(), actual.Id.ToString());
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.ActivityIds.Select(id => id.ToString()), actual.ActivityIds.Select(k => k.ToString()));
            Assert.Equal(expected.TagIds.Select(id => id.ToString()), actual.TagIds.Select(k => k.ToString()));

            Assert.Equal(expected.Periods.Count, actual.Periods.Count);
            for (var j = 0; j < expected.Periods.Count; j++)
            {
                Assert.Equal(expected.Periods[j].Period, actual.Periods[j].Period);
                Assert.Equal(expected.Periods[j].TimeZoneOffset, actual.Periods[j].TimeZoneOffset);
                Assert.Equal(expected.Periods[j].Description, actual.Periods[j].Description);
            }
        }
    }
}
