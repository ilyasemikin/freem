using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.DTO.Records.Create;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsCreateUseCase : UseCaseTestBase
{
    private const string RecordName = "name";
    private const string RecordDescription = "description";

    private const string AnotherUserLogin = "another_user";
    
    private readonly UseCaseExecutionContext _context;

    private readonly DateTimePeriod _period;
    private readonly RelatedActivitiesCollection _activities;
    
    public RecordsCreateUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        
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
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
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
        var notExistedActivityId = Services.Generators.CreateActivityIdentifier();
        var activities = new RelatedActivitiesCollection([notExistedActivityId]);

        var request = new CreateRecordRequest(_period, activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);

        Assert.Equal(CreateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenActivitiesBelongToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var activity = Services.Samples.Activities.Create(anotherUserId);
        var activities = new RelatedActivitiesCollection([activity]);

        var request = new CreateRecordRequest(_period, activities);
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);
        
        Assert.Equal(CreateRecordErrorCode.RelatedActivitiesNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsDoesNotExist()
    {
        var notExistedTagId = Services.Generators.CreateTagIdentifier();
        var tags = new RelatedTagsCollection([notExistedTagId]);

        var request = new CreateRecordRequest(_period, _activities)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.False(response.Success);
        Assert.Null(response.Record);
        Assert.NotNull(response.Error);
        
        Assert.Equal(CreateRecordErrorCode.RelatedTagsNotFound, response.Error.Code);
    }

    [Fact]
    public async Task ShouldFailure_WhenTagsBelongToAnotherUser()
    {
        var anotherUserId = Services.Samples.Users.Register(AnotherUserLogin);
        var tag = Services.Samples.Tags.Create(anotherUserId);
        var tags = new RelatedTagsCollection([tag]);

        var request = new CreateRecordRequest(_period, _activities)
        {
            Tags = tags
        };
        
        var response = await Services.RequestExecutor.ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(_context, request);
        
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
        
        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<CreateRecordRequest, CreateRecordResponse>(UseCaseExecutionContext.Empty, request));
        
        Assert.IsType<UnauthorizedException>(exception);
    }
}