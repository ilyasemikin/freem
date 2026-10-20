using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Records.Create;
using Freem.Domain.UseCases.IntegrationTests.Fixtures;
using Freem.Domain.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.IntegrationTests.Tests;

public sealed class RecordsCreateUseCase : UseCaseTestBase
{
    private static readonly RecordName RecordName = RecordName.Parse("name", null);
    private static readonly RecordDescription RecordDescription = RecordDescription.Parse("description", null);
    
    private HandlerContext _context = null!;

    private DateTimePeriod _period = null!;
    private RelatedActivityIdsCollection _activities = null!;
    
    public RecordsCreateUseCase(TestContext context) 
        : base(context)
    {
    }

    public override async Task InitializeAsync()
    {
        using var filler = Context.CreateExecutor();
        
        var userId = await filler.Users.RegisterAsync();
        _context = new HandlerContext(userId);
        
        var activity = await filler.Activities.CreateAsync(_context);
        
        var now = DateTime.UtcNow;
        _period = new DateTimePeriod(now.AddHours(-4), now);
        _activities = new RelatedActivityIdsCollection([activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateRecordRequest(_period, _activities)
        {
            Name = RecordName,
            Description = RecordDescription,
        };
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Value.Record);
        Assert.Null(response.Error);
        
        Assert.Equal(_context.UserId, response.Value.Record.UserId);
        Assert.Equal(RecordName, response.Value.Record.Name);
        Assert.Equal(RecordDescription, response.Value.Record.Description);
        Assert.Equal(_activities, response.Value.Record.Activities, IReadOnlyRelatedIdsCollection<ActivityId>.Equals);
    }

    [Fact]
    public async Task ShouldSuccess_WhenSeveralPeriods()
    {
        var description = RecordPeriodDescription.Parse("second", null);
        var periods = new[]
        {
            new NewRecordPeriod(_period),
            new NewRecordPeriod(new DateTimePeriod(_period.EndAt.AddHours(1), _period.EndAt.AddHours(2))) { Description = description }
        };
        var request = new CreateRecordRequest(periods, _activities);

        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);

        Assert.True(response.Success);

        using var executor = Context.CreateExecutor();
        var actual = await executor.Records.RequiredGetAsync(_context, response.Value.Record.Id);
        Assert.Equal(2, actual.Periods.Count);
        Assert.Equal(_period, actual.Periods[0].Period);
        Assert.Equal(description, actual.Periods[1].Description);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodsOverlap()
    {
        var periods = new[]
        {
            new NewRecordPeriod(_period),
            new NewRecordPeriod(new DateTimePeriod(_period.EndAt.AddMinutes(-1), _period.EndAt.AddHours(1)))
        };
        var request = new CreateRecordRequest(periods, _activities);

        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenNoPeriods()
    {
        var request = new CreateRecordRequest([], _activities);

        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);

        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenPeriodHasZeroDuration()
    {
        var period = new DateTimePeriod(_period.StartAt, _period.StartAt);
        var request = new CreateRecordRequest(period, _activities);

        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);

        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Equal(DomainErrorCatalog.Types.PeriodIsInvalid, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = ActivityId.Generate();
        var activities = new RelatedActivityIdsCollection([notExistedActivityId]);

        var request = new CreateRecordRequest(_period, activities);
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);

        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = await executor.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var activity = await executor.Activities.CreateAsync(anotherContext);
        
        var activities = new RelatedActivityIdsCollection([activity.Id]);

        var request = new CreateRecordRequest(_period, activities);
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = TagId.Generate();
        var tags = new RelatedTagIdsCollection([notExistedTagId]);

        var request = new CreateRecordRequest(_period, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = await executor.Users.RegisterAsync();
        var anotherContext = new HandlerContext(anotherUserId);
        var tag = await executor.Tags.CreateAsync(anotherContext);
        
        var tags = new RelatedTagIdsCollection([tag.Id]);

        var request = new CreateRecordRequest(_period, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, Result<CreateRecordResponse, Error>>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Value);
        Assert.NotNull(response.Error);
        
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, response.Error.Type);
    }
    
}