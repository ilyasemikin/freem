using Freem.Api.Public.Contracts.V1.Records.Running.Dtos;
using Freem.Api.Public.Contracts.V1.Records.Dtos;
using System.Text.Json;
using Freem.Domain.Records;
using Freem.Api.Public.Client.Implementations.Base;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Records.Running;
using Utilities.Http;
using Utilities.Http.Errors.Abstractions;
using Utilities.Http.Extensions;
using Utilities.Primitives;

namespace Freem.Api.Public.Client.Implementations;

public sealed class RecordsClient : BaseClient
{
    private readonly JsonSerializerOptions _options;
    
    public RecordsClient(IHttpClient client, JsonSerializerOptions options, ClientSession session) 
        : base(client, options, session)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;
    }

    public Task<Result<RecordCreateResponse, IClientError>> CreateAsync(RecordCreateRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/records")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync<RecordCreateResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateAsync(
        RecordId id, 
        RecordUpdateRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/records/{id}")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> RemoveAsync(RecordId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/records/{id}");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<RecordPeriodAddResponse, IClientError>> AddPeriodAsync(
        RecordId recordId,
        RecordPeriodItem body,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post($"api/v1/records/{recordId}/periods")
            .WithJsonBody(body, _options);

        return SendForUserAsync<RecordPeriodAddResponse>(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdatePeriodAsync(
        RecordId recordId,
        RecordPeriodId periodId,
        RecordPeriodUpdateRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put($"api/v1/records/{recordId}/periods/{periodId}")
            .WithJsonBody(body, _options);

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> RemovePeriodAsync(
        RecordId recordId,
        RecordPeriodId periodId,
        CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete($"api/v1/records/{recordId}/periods/{periodId}");

        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> StartAsync(RunningRecordStartRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/records/running/start")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> StopAsync(RunningRecordStopRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Post("api/v1/records/running/stop")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> UpdateRunningAsync(
        RunningRecordUpdateRequest body, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Put("api/v1/records/running")
            .WithJsonBody(body, _options);
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<IClientError>> RemoveRunningAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Delete("api/v1/records/running");
        
        return SendForUserAsync(request, cancellationToken);
    }

    public Task<Result<RunningRecordResponse, IClientError>> GetRunningAsync(CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records/running");
        
        return SendForUserAsync<RunningRecordResponse>(request, cancellationToken);
    }

    public Task<Result<RecordResponse, IClientError>> GetAsync(
        RecordId id, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records/{id}");
        
        return SendForUserAsync<RecordResponse>(request, cancellationToken);
    }

    public Task<Result<RecordListResponse, IClientError>> ListAsync(
        RecordListRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Offset), query.Offset.ToString());
        
        return SendForUserAsync<RecordListResponse>(request, cancellationToken);
    }

    public Task<Result<RecordListResponse, IClientError>> ListByPeriodAsync(
        RecordListByPeriodRequest query, CancellationToken cancellationToken = default)
    {
        var request = HttpRequest.Get($"api/v1/records/by-period")
            .WithQueryParameter(nameof(query.Limit), query.Limit.ToString())
            .WithQueryParameter(nameof(query.Period), query.Period.ToString());
        
        return SendForUserAsync<RecordListResponse>(request, cancellationToken);
    }
}