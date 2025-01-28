using Freem.Entities.Statistics.Activities;

namespace Freem.Entities.Statistics;

public sealed class StatisticsPerDay
{
    public DateOnly Day { get; }
    
    public TimeSpan Duration { get; }
    public ActivitiesStatistics ActivitiesStatistics { get; }
    
    public StatisticsPerDay(DateOnly day, TimeSpan duration, ActivitiesStatistics activitiesStatistics)
    {
        ArgumentNullException.ThrowIfNull(day);
        ArgumentNullException.ThrowIfNull(duration);
        ArgumentNullException.ThrowIfNull(activitiesStatistics);
        
        Day = day;
        Duration = duration;
        ActivitiesStatistics = activitiesStatistics;
    }
}