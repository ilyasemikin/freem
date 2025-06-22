using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Activities;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Activities.Find;

public sealed class FindActivityResponse : IResponse<FindActivityErrorCode>
{
    [MemberNotNullWhen(true, nameof(Activities))]
    [MemberNotNullWhen(true, nameof(TotalCount))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }

    public IReadOnlyList<Activity>? Activities { get; }
    public TotalCount? TotalCount { get; }

    public Error<FindActivityErrorCode>? Error { get; }

    private FindActivityResponse(
        IReadOnlyList<Activity>? activities = null,
        TotalCount? totalCount = null,
        Error<FindActivityErrorCode>? error = null)
    {
        Success = activities is not null;
        
        Activities = activities;
        TotalCount = totalCount;
        Error = error;
    }

    public static FindActivityResponse CreateSuccess(IReadOnlyList<Activity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(totalCount);
        ArgumentNullException.ThrowIfNull(activities);

        return new FindActivityResponse(activities, totalCount);
    }
}