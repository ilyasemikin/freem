using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Activities.List;

public sealed class ListActivityResponse : IResponse<ListActivityErrorCode>
{
    [MemberNotNullWhen(true, nameof(Activities))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public IReadOnlyList<Activity>? Activities { get; }
    public TotalCount? TotalCount { get; }
    
    public Error<ListActivityErrorCode>? Error { get; }

    private ListActivityResponse(
        IReadOnlyList<Activity>? activities = null, 
        TotalCount? totalCount = null,
        Error<ListActivityErrorCode>? error = null)
    {
        Success = activities is not null;
        
        Activities = activities;
        TotalCount = totalCount;
        Error = error;
    }

    public static ListActivityResponse CreateSuccess(IReadOnlyList<Activity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(totalCount);
        
        return new ListActivityResponse(activities, totalCount);
    }

    public static ListActivityResponse CreateFailure(ListActivityErrorCode code, string? message = null)
    {
        var error = new Error<ListActivityErrorCode>(code, message);
        return new ListActivityResponse(error: error);
    }
}