namespace Freem.Web.Api.Public.SyncClient.Implementations;

public sealed class CompositeSyncClient
{
    public ActivitiesSyncClient Activities { get; }
    public RecordsSyncClient Records { get; }
    public TagsSyncClient Tags { get; }
    
    public EventsSyncClient Events { get; }
    public StatisticsSyncClient Statistics { get; }
    
    public UsersSyncClient Users { get; }
    
    public CompositeSyncClient(
        ActivitiesSyncClient activities, 
        RecordsSyncClient records, 
        TagsSyncClient tags, 
        EventsSyncClient events, 
        StatisticsSyncClient statistics, 
        UsersSyncClient users)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(statistics);
        ArgumentNullException.ThrowIfNull(users);
        
        Activities = activities;
        Records = records;
        Tags = tags;
        Events = events;
        Statistics = statistics;
        Users = users;
    }
}