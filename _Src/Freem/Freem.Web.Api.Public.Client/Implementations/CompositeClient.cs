namespace Freem.Web.Api.Public.Client.Implementations;

public sealed class CompositeClient
{
    public ActivitiesClient Activities { get; }
    public RecordsClient Records { get; }
    public TagsClient Tags { get; }
    
    public EventsClient Events { get; }
    public StatisticsClient Statistics { get; }
    
    public UsersClient Users { get; }
    
    public CompositeClient(
        ActivitiesClient activities, 
        RecordsClient records, 
        TagsClient tags, 
        EventsClient events, 
        StatisticsClient statistics, 
        UsersClient users)
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