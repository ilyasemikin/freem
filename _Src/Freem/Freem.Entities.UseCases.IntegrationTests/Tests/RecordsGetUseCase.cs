using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Comparers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Entities.UseCases.Records.Get.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsGetUseCase : UseCaseTestBase
{
    private readonly UseCaseExecutionContext _context;
    private readonly Entities.Records.Record _record;
    
    public RecordsGetUseCase(ServicesContext services) 
        : base(services)
    {
        var userId = services.Samples.Users.Register();
        var activity = services.Samples.Activities.Create(userId);
        var record = services.Samples.Records.Create(userId, activity.Id);

        _context = new UseCaseExecutionContext(userId);
        _record = record;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetRecordRequest(_record.Id);

        var response = await Services.RequestExecutor.ExecuteAsync<GetRecordRequest, GetRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Founded);
        Assert.NotNull(response.Record);
        
        Assert.Equal(_record, response.Record, new RecordEqualityComparer());
    }
}