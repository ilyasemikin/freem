﻿using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Models.Fields;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Entities.UseCases.Records.Update.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsUpdateUseCaseTests : UseCaseTestBase
{
    private const string UpdatedName = "record";
    private const string UpdatedDescription = "description";

    private readonly UseCaseExecutionContext _context;
    private readonly RecordIdentifier _recordId;
    private readonly ActivityIdentifier _updatedActivityId;
    private readonly TagIdentifier _updatedTagId;
    
    public RecordsUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        var record = services.Samples.Records.Create(userId, activity.Id);

        var updatedActivity = services.Samples.Activities.Create(userId);
        var updateTag = services.Samples.Tags.Create(userId);
        
        _context = new UseCaseExecutionContext(userId);
        _recordId = record.Id;
        _updatedActivityId = updatedActivity.Id;
        _updatedTagId = updateTag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Name = new UpdateField<RecordName>(UpdatedName)
        };

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateDescription()
    {
        var request = new UpdateRecordRequest(_recordId)
        {
            Description = new UpdateField<RecordDescription>(UpdatedDescription)
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateActivities()
    {
        var activities = new RelatedActivitiesCollection([_updatedActivityId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Activities = new UpdateField<RelatedActivitiesCollection>(activities)
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagsCollection([_updatedTagId]);
        var request = new UpdateRecordRequest(_recordId)
        {
            Tags = tags
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}