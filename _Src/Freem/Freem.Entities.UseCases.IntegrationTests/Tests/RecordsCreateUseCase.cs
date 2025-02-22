using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Contracts.Records.Create;
using Freem.Entities.UseCases.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsCreateUseCase : UseCaseTestBase
{
    private const string RecordName = "name";
    private const string RecordDescription = "description";
    
    private readonly UseCaseExecutionContext _context;

    private readonly DateTimePeriod _period;
    private readonly RelatedActivitiesCollection _activities;
    
    public RecordsCreateUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.UsersPassword.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        
        var now = DateTime.UtcNow;
        _period = new DateTimePeriod(now.AddHours(-4), now);
        _activities = new RelatedActivitiesCollection([activity.Id]);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new CreateRecordRequest(_period, _activities)
        {
            Name = RecordName,
            Description = RecordDescription,
        };
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Record);
        Assert.Null(response.Error);
        
        Assert.Equal(_context.UserId, response.Record.UserId);
        Assert.Equal(RecordName, response.Record.Name);
        Assert.Equal(RecordDescription, response.Record.Description);
        Assert.Equal(_activities, response.Record.Activities, IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);

        var request = new CreateRecordRequest(_period, activities);
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);

        Assert.Equal(CreateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivitiesCollection([activity]);

        var request = new CreateRecordRequest(_period, activities);
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);
        
        Assert.Equal(CreateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Context.CreateIdentifier<TagIdentifier>();
        var tags = new RelatedTagsCollection([notExistedTagId]);

        var request = new CreateRecordRequest(_period, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);
        
        Assert.Equal(CreateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongToAnotherUser()
    {
        using var executor = Context.CreateExecutor();
        var anotherUserId = executor.UsersPassword.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagsCollection([tag]);

        var request = new CreateRecordRequest(_period, _activities)
        {
            Tags = tags
        };
        
        var response = await Context.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);
        
        Assert.Equal(CreateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new CreateRecordRequest(_period, _activities);
        
        var exception = await Record.ExceptionAsync(async () => await Context
            .ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}