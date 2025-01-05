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
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private const string ActivityName = "activity";
    
    private readonly UseCaseExecutionContext _context;
    private readonly Entities.Records.Record _record;
    
    public RecordsGetUseCase(ServicesContext context) 
        : base(context)
    {
        var registerRequest = new RegisterUserPasswordRequest(Nickname, Login, Password);
        var registerResponse = Context.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, registerRequest);

        _context = new UseCaseExecutionContext(registerResponse.UserId);

        var activityRequest = new CreateActivityRequest(ActivityName);
        var activityResponse = Context.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(_context, activityRequest);

        var now = DateTime.UtcNow;
        var period = new DateTimePeriod(now.AddHours(-1), now);
        var activities = new RelatedActivitiesCollection([activityResponse.Activity]);
        var recordRequest = new CreateRecordRequest(period, activities);
        var recordResponse = Context.RequestExecutor.Execute<CreateRecordRequest, CreateRecordResponse>(_context, recordRequest);
        _record = recordResponse.Record;
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new GetRecordRequest(_record.Id);

        var response = await Context.RequestExecutor.ExecuteAsync<GetRecordRequest, GetRecordResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Founded);
        Assert.NotNull(response.Record);
        
        Assert.Equal(_record, response.Record, new RecordEqualityComparer());
    }
}