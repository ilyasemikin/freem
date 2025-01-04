using System.Collections;
using Freem.Entities.Activities;
using Freem.Entities.UseCases.Models.Filter;

namespace Freem.Entities.UseCases.Activities.List.Models;

public sealed class ListActivityResponse : IEnumerable<Activity>
{
    public IReadOnlyList<Activity> Activities { get; }
    public TotalCount TotalCount { get; }

    public ListActivityResponse(IReadOnlyList<Activity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(totalCount);
        
        Activities = activities;
        TotalCount = totalCount;
    }
    
    public ListActivityResponse(IEnumerable<Activity> activities, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(totalCount);
        
        Activities = activities.ToArray();
        TotalCount = totalCount;
    }

    public IEnumerator<Activity> GetEnumerator()
    {
        return Activities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}