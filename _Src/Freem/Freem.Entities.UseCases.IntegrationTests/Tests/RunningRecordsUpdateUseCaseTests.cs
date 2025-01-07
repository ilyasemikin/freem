using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Models.Fields;
using Freem.Entities.UseCases.RunningRecords.Update.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "record_name";
    private const string UpdatedDescription = "record_description";
    
    private readonly UseCaseExecutionContext _context;
    private readonly ActivityIdentifier _updatedActivityId;
    private readonly TagIdentifier _updatedTagId;
    
    public RunningRecordsUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        services.Samples.RunningRecords.Start(userId, activity.Id);

        var updateActivity = services.Samples.Activities.Create(userId);
        var updateTag = services.Samples.Tags.Create(userId);
        
        _context = new UseCaseExecutionContext(userId);
        _updatedActivityId = updateActivity.Id;
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRunningRecordRequest
        {
            Name = new UpdateField<RecordName>(UpdatedName)
        };

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRunningRecordRequest
        {
            Description = new UpdateField<RecordDescription>(UpdatedDescription)
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateActivities()
    {
        var activities = new RelatedActivitiesCollection([_updatedActivityId]);
        var request = new UpdateRunningRecordRequest
        {
            Activities = new UpdateField<RelatedActivitiesCollection>(activities)
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagsCollection([_updatedTagId]);
        var request = new UpdateRunningRecordRequest
        {
            Tags = new UpdateField<RelatedTagsCollection>(tags)
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}