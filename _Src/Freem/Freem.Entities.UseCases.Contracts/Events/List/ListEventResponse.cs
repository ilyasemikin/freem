using System.Diagnostics.CodeAnalysis;
using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Events.List;

public sealed class ListEventResponse : IResponse<ListEventErrorCode>
{
    [MemberNotNullWhen(true, nameof(Events))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyList<IEntityEvent<Id, UserId>>? Events { get; }
    public TotalCount? TotalCount { get; }
    
    public Error<ListEventErrorCode>? Error { get; }
    
    private ListEventResponse(
        IReadOnlyList<IEntityEvent<Id, UserId>>? events = null, 
        TotalCount? totalCount = null,
        Error<ListEventErrorCode>? error = null)
    {
        Success = error is null;
        Events = events;
        TotalCount = totalCount;
        Error = error;
    }

    public static ListEventResponse CreateSuccess(
        IReadOnlyList<IEntityEvent<Id, UserId>> events, 
        TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(totalCount);

        return new ListEventResponse(events, totalCount);
    }
    
    public static ListEventResponse CreateFailure(ListEventErrorCode code)
    {
        var error = new Error<ListEventErrorCode>(code);
        return new ListEventResponse(error: error);
    }
}