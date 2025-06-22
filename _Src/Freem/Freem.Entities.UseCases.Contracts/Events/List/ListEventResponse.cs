using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.Users.Identifiers;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Events.List;

public sealed class ListEventResponse : IResponse<ListEventErrorCode>
{
    [MemberNotNullWhen(true, nameof(Events))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyList<IEntityEvent<IEntityIdentifier, UserIdentifier>>? Events { get; }
    public TotalCount? TotalCount { get; }
    
    public Error<ListEventErrorCode>? Error { get; }
    
    private ListEventResponse(
        IReadOnlyList<IEntityEvent<IEntityIdentifier, UserIdentifier>>? events = null, 
        TotalCount? totalCount = null,
        Error<ListEventErrorCode>? error = null)
    {
        Success = error is null;
        Events = events;
        TotalCount = totalCount;
        Error = error;
    }

    public static ListEventResponse CreateSuccess(
        IReadOnlyList<IEntityEvent<IEntityIdentifier, UserIdentifier>> events, 
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