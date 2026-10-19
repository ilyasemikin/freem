using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
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
    private readonly RelatedActivitiesIdentifiersCollection _activities;
    
    public RecordsCreateUseCase(TestContext context) 
        : base(context)
    {
        using var filler = Context.CreateExecutor();
        
        var userId = filler.Users.Register();
        _context = new UseCaseExecutionContext(userId);
        
        var activity = filler.Activities.Create(_context);
        
        var now = DateTime.UtcNow;
        _period = new DateTimePeriod(now.AddHours(-4), now);
        _activities = new RelatedActivitiesIdentifiersCollection([activity.Id]);
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
        Assert.Equal(_activities, response.Record.Activities, IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier>.Equals);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesDoesNotExist()
    {
        var notExistedActivityId = Context.CreateIdentifier<ActivityIdentifier>();
        var activities = new RelatedActivitiesIdentifiersCollection([notExistedActivityId]);

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
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var activity = executor.Activities.Create(anotherContext);
        
        var activities = new RelatedActivitiesIdentifiersCollection([activity.Id]);

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
        var tags = new RelatedTagsIdentifiersCollection([notExistedTagId]);

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
        var anotherUserId = executor.Users.Register();
        var anotherContext = new UseCaseExecutionContext(anotherUserId);
        var tag = executor.Tags.Create(anotherContext);
        
        var tags = new RelatedTagsIdentifiersCollection([tag.Id]);

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