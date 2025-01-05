using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Comparers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Activities.Create.Models;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Records.Create.Models;
using Freem.Entities.UseCases.Records.List.Models;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class RecordsListUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";

    private const string ActivityName = "activity";
    private const int RecordsCount = 10;
    
    private readonly UseCaseExecutionContext _context;
    private readonly IReadOnlyList<Entities.Records.Record> _records;
    
    public RecordsListUseCaseTests(ServicesContext context) 
        : base(context)
    {
        var registerRequest = new RegisterUserPasswordRequest(Nickname, Login, Password);
        var registerResponse = Context.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, registerRequest);

        _context = new UseCaseExecutionContext(registerResponse.UserId);

        var activityRequest = new CreateActivityRequest(ActivityName);
        var activityResponse = Context.RequestExecutor.Execute<CreateActivityRequest, CreateActivityResponse>(_context, activityRequest);

        var records = new Entities.Records.Record[RecordsCount];
        foreach (var index in Enumerable.Range(0, RecordsCount))
        {
            var now = DateTimeOffset.UtcNow;
            var period = new DateTimePeriod(now.AddHours(-1), now);
            var activities = new RelatedActivitiesCollection([activityResponse.Activity.Id]);

            var recordRequest = new CreateRecordRequest(period, activities);
            var recordResponse = Context.RequestExecutor.Execute<CreateRecordRequest, CreateRecordResponse>(_context, recordRequest);
            
            records[index] = recordResponse.Record;
        }

        _records = records
            .OrderBy(e => (string)e.Id)
            .ToArray();
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new ListRecordRequest();
        
        var response = await Context.RequestExecutor.ExecuteAsync<ListRecordRequest, ListRecordResponse>(_context, request);

        Assert.NotNull(response);

        var orderedRecords = response.Records
            .OrderBy(record => (string)record.Id)
            .ToArray();
        
        Assert.Equal(RecordsCount, orderedRecords.Length);
        Assert.Equal(_records, orderedRecords, new RecordEqualityComparer());
    }
}