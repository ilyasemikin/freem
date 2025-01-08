using Freem.Entities.RunningRecords;
using Freem.Entities.RunningRecords.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Exceptions;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.RunningRecords.Get.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RunningRecordsGetUseCaseTests : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly RunningRecord _record;
    
    public RunningRecordsGetUseCaseTests(ServicesContext services) : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        var record = services.Samples.RunningRecords.Start(userId, activity.Id);
        
        _context = new UseCaseExecutionContext(userId);
        _record = record;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetRunningRecordRequest();

        var response = await Services.RequestExecutor.ExecuteAsync<GetRunningRecordRequest, GetRunningRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Record);
        Assert.Null(response.Error);

        Assert.Equal(_record, response.Record, new RunningRecordEqualityComparer());
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUnauthorized()
    {
        var request = new GetRunningRecordRequest();

        var exception = await Record.ExceptionAsync(async () => await Services.RequestExecutor
            .ExecuteAsync<GetRunningRecordRequest, GetRunningRecordResponse>(UseCaseExecutionContext.Empty, request));

        Assert.IsType<UnauthorizedException>(exception);
    }
}