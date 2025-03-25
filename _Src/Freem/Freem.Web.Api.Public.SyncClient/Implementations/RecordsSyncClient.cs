using System.Text.Json;
using Freem.Entities.Records.Identifiers;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.Contracts.Records.Running;
using Freem.Web.Api.Public.SyncClient.Implementations.Base;

namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class RecordsSyncClient : BaseSyncClient
{
    private readonly JsonSerializerOptions _options;
    
    public RecordsSyncClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public ClientResult<CreateRecordResponse> Create(CreateRecordRequest body)
    {
        var request = HttpRequest.Post("api/v1/records")
            .WithJsonBody(body, _options);

        return SendAuthorized<CreateRecordResponse>(request);
    }

    public ClientResult Update(RecordIdentifier id, UpdateRecordRequest body)
    {
        var request = HttpRequest.Put($"api/v1/records/{id}")
            .WithJsonBody(body, _options);

        return SendAuthorized(request);
    }

    public ClientResult Remove(RecordIdentifier id)
    {
        var request = HttpRequest.Delete($"api/v1/records/{id}");
        
        return SendAuthorized(request);
    }

    public ClientResult Start(StartRunningRecordRequest body)
    {
        var request = HttpRequest.Post("api/v1/records/running/start")
            .WithJsonBody(body, _options);

        return SendAuthorized(request);
    }

    public ClientResult Stop(StopRunningRecordRequest body)
    {
        var request = HttpRequest.Post("api/v1/records/running/stop")
            .WithJsonBody(body, _options);
        
        return SendAuthorized(request);
    }

    public ClientResult UpdateRunning(UpdateRunningRecordRequest body)
    {
        var request = HttpRequest.Put("api/v1/records/running")
            .WithJsonBody(body, _options);

        return SendAuthorized(request);
    }

    public ClientResult RemoveRunning()
    {
        var request = HttpRequest.Delete("api/v1/records/running");
        
        return SendAuthorized(request);
    }
    
    public ClientResult<RunningRecordResponse> GetRunning()
    {
        var request = HttpRequest.Get($"api/v1/records/running");
        
        return SendAuthorized<RunningRecordResponse>(request);
    }

    public ClientResult<RunningRecordResponse> Get(RecordIdentifier id)
    {
        var request = HttpRequest.Get($"api/v1/records/{id}");
        
        return SendAuthorized<RunningRecordResponse>(request);
    }

    public ClientResult<IAsyncEnumerable<RunningRecordResponse>> List(ListRecordRequest query)
    {
        var request = HttpRequest.Get($"api/v1/records")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());

        return SendAuthorized<IAsyncEnumerable<RunningRecordResponse>>(request);
    }

    public ClientResult<IAsyncEnumerable<RunningRecordResponse>> ListByPeriod(ListRecordByPeriodRequest query)
    {
        var request = HttpRequest.Get($"api/v1/records/by-period")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());

        return SendAuthorized<IAsyncEnumerable<RunningRecordResponse>>(request);
    }
}