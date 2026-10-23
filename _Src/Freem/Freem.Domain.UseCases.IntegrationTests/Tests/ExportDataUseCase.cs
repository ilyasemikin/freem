using Freem.Domain.Activities.Models;
using Freem.Domain.Formats;
using Freem.Domain.Relations.Collections;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Export;
using Freem.Domain.UseCases.Contracts.Formats.List;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.Import;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class ExportDataUseCase : UseCaseTestBase
{
    private static readonly FormatKey NativeJsonKey = FormatKey.Parse("freem-json", null);

    private HandlerContext _context = null!;
    private IReadOnlyList<Domain.Entities.Tags.Tag> _tags = null!;
    private IReadOnlyList<Domain.Entities.Activities.Activity> _activities = null!;
    private Domain.Entities.Records.Record _record = null!;

    public ExportDataUseCase(TestContext context)
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();

        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);

        _tags = await filler.Tags.CreateManyAsync(_context, 3);
        _activities = await filler.Activities.CreateManyAsync(_context, 2);
        await filler.Activities.ArchiveAsync(_context, _activities[1].Id);

        var now = DateTimeOffset.UtcNow;
        var firstPeriod = new NewRecordPeriod(new DateTimePeriod(now.AddHours(-5), now.AddHours(-4)));
        var secondPeriod = new NewRecordPeriod(new DateTimePeriod(now.AddHours(-2), now.AddHours(-1)));
        var activityIds = new RelatedActivityIdsCollection(_activities.Select(a => a.Id));
        var request = new CreateRecordRequest([firstPeriod, secondPeriod], activityIds)
        {
            Tags = new RelatedTagIdsCollection([_tags[0].Id])
        };
        _record = await filler.Records.CreateAsync(_context, request);
    }

    [Fact]
    public async Task ShouldExportAllUserData_InNativeJson()
    {
        using var output = new MemoryStream();
        var request = new ExportDataRequest(NativeJsonKey, _ => output);

        var response = await Context.ExecuteAsync<ExportDataRequest, Result<ExportDataResponse, Error>>(_context, request);

        Assert.True(response.Success);
        Assert.Equal(NativeJsonKey, response.Value.Format.Key);

        output.Position = 0;
        var parsed = await new NativeJsonImportParser().ParseAsync(output);
        Assert.True(parsed.Success);

        var document = parsed.Value;
        Assert.Equal(_tags.Select(t => t.Id.ToString()).Order(), document.Tags.Select(t => t.Id.ToString()).Order());
        Assert.Equal(_activities.Select(a => a.Id.ToString()).Order(), document.Activities.Select(a => a.Id.ToString()).Order());

        var archived = document.Activities.Single(a => a.Id.ToString() == _activities[1].Id.ToString());
        Assert.Equal(ActivityStatus.Archived, archived.Status);

        var record = Assert.Single(document.Records);
        Assert.Equal(_record.Id.ToString(), record.Id.ToString());
        Assert.Equal(2, record.Periods.Count);
        Assert.Equal([_tags[0].Id.ToString()], record.TagIds.Select(k => k.ToString()));
    }

    [Fact]
    public async Task ShouldExportOnlyDataOfRequestedUser()
    {
        using var filler = Context.CreateExecutor();
        var otherUserId = await filler.Users.RegisterAsync();
        var otherContext = new HandlerContext(otherUserId);

        using var output = new MemoryStream();
        var request = new ExportDataRequest(NativeJsonKey, _ => output);
        var response = await Context.ExecuteAsync<ExportDataRequest, Result<ExportDataResponse, Error>>(otherContext, request);

        Assert.True(response.Success);
        output.Position = 0;
        var parsed = await new NativeJsonImportParser().ParseAsync(output);
        Assert.True(parsed.Success);
        Assert.Empty(parsed.Value.Tags);
        Assert.Empty(parsed.Value.Activities);
        Assert.Empty(parsed.Value.Records);
    }

    [Fact]
    public async Task ShouldReturnFormatNotFound_WhenFormatIsUnknown()
    {
        var opened = false;
        var request = new ExportDataRequest(FormatKey.Parse("unknown", null), _ =>
        {
            opened = true;
            return Stream.Null;
        });

        var response = await Context.ExecuteAsync<ExportDataRequest, Result<ExportDataResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.FormatNotFound, response.Error.Type);
        Assert.False(opened);
    }

    [Fact]
    public async Task ListFormats_ShouldContainNativeJson()
    {
        var request = new ListFormatsRequest();

        var response = await Context.ExecuteAsync<ListFormatsRequest, Result<ListFormatsResponse, Error>>(_context, request);

        Assert.True(response.Success);
        Assert.Contains(response.Value.ExportFormats, format => format.Key == NativeJsonKey);
        Assert.Contains(response.Value.ImportFormats, format => format.Key == NativeJsonKey);
    }
}
