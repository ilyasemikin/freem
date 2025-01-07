using Freem.Entities.Activities;
using Freem.Entities.Activities.Models;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.Activities.Update.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Models.Fields;
using Freem.Entities.UseCases.Users.Password.Register.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class ActivitiesUpdateUseCaseTests : UseCaseTestBase
{
    private const string NewName = "updated_activity";
    
    private readonly UseCaseExecutionContext _context;
    private readonly Activity _activity;
    private readonly TagIdentifier _updatedTagId;
    
    public ActivitiesUpdateUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        
        var tag = services.Samples.Tags.Create(userId);

        _context = new UseCaseExecutionContext(userId);
        _activity = activity;
        _updatedTagId = tag.Id;
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateName()
    {
        var request = new UpdateActivityRequest(_activity.Id)
        {
            Name = new UpdateField<ActivityName>(NewName)
        };

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }

    [Fact]
    public async Task ShouldSuccess_WhenUpdateTags()
    {
        var tags = new RelatedTagsCollection([_updatedTagId]);
        var request = new UpdateActivityRequest(_activity.Id)
        {
            Tags = tags
        };
        
        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}