using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Filter;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.DTO.Events.List;

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
    
    public static ListEventResponse CreateFailure(ListEventErrorCode code, string? message = null)
    {
        var error = new Error<ListEventErrorCode>(code, message);
        return new ListEventResponse(error: error);
    }
}