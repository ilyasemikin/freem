using System.Text.Json;
using Freem.Entities.Records.Identifiers;
using Freem.Http.Requests.Abstractions;
using Freem.Http.Requests.Entities;
using Freem.Http.Requests.Entities.Extensions;
using Freem.Web.Api.Public.Client.Implementations.Base;
using Freem.Web.Api.Public.Client.Models;
using Freem.Web.Api.Public.Contracts.DTO.Records;
using Freem.Web.Api.Public.Contracts.DTO.Records.Running;

namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class RecordsClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public RecordsClient(IHttpClient client, JsonSerializerOptions options, TokenLoader tokenLoader) 
        : base(client, options, tokenLoader)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<ClientResult<CreateRecordResponse>> CreateAsync(CreateRecordRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/records")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync<CreateRecordResponse>(request, cancellationToken);
    }

    public Task<ClientResult> UpdateAsync(
        RecordIdentifier id, 
        UpdateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/records/{id}")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> RemoveAsync(RecordIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/records/{id}");
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> StartAsync(StartRunningRecordRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/records/running/start")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> StopAsync(StopRunningRecordRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/records/running/stop")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> UpdateRunningAsync(
        UpdateRunningRecordRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put("api/v1/records/running")
            .WithJsonBody(body, _options);
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult> RemoveRunningAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete("api/v1/records/running");
        
        return SendAuthorizedAsync(request, cancellationToken);
    }

    public Task<ClientResult<RunningRecordResponse>> GetRunningAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records/running");
        
        return SendAuthorizedAsync<RunningRecordResponse>(request, cancellationToken);
    }

    public Task<ClientResult<RecordResponse>> GetAsync(
        RecordIdentifier id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records/{id}");
        
        return SendAuthorizedAsync<RecordResponse>(request, cancellationToken);
    }

    public Task<ClientResult<IAsyncEnumerable<RunningRecordResponse>>> ListAsync(
        ListRecordRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendAuthorizedAsync<IAsyncEnumerable<RunningRecordResponse>>(request, cancellationToken);
    }

    public Task<ClientResult<IAsyncEnumerable<RunningRecordResponse>>> ListByPeriodAsync(
        ListRecordByPeriodRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records/by-period")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());
        
        return SendAuthorizedAsync<IAsyncEnumerable<RunningRecordResponse>>(request, cancellationToken);
    }
}