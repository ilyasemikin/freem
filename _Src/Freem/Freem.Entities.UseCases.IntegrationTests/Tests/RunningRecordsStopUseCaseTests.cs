using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.RunningRecords.Stop.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsStopUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    
    public RunningRecordsStopUseCaseTests(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        services.Samples.RunningRecords.Start(userId, activity.Id);

        _context = new UseCaseExecutionContext(userId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var now = DateTimeOffset.UtcNow;
        
        var request = new StopRunningRecordRequest(now);

        await Services.RequestExecutor.ExecuteAsync(_context, request);
    }
}