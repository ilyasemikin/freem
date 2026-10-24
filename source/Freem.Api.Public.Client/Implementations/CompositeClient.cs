namespace Freem.Api.Public.Client.Implementations;

public sealed class CompositeClient
{
    public ActivitiesClient Activities { get; }
    public RecordsClient Records { get; }
    public TagsClient Tags { get; }
    
    public EventsClient Events { get; }
    public StatisticsClient Statistics { get; }
    public DataTransferClient DataTransfer { get; }
    public ImportsClient Imports { get; }
    
    public UsersClient Users { get; }
    public AuthClient Auth { get; }

    public HealthClient Health { get; }
    public MetaClient Meta { get; }
    
    public CompositeClient(
        ActivitiesClient activities, 
        RecordsClient records, 
        TagsClient tags, 
        EventsClient events, 
        StatisticsClient statistics, 
        DataTransferClient dataTransfer,
        ImportsClient imports,
        UsersClient users,
        AuthClient auth,
        HealthClient health,
        MetaClient meta)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(records);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(statistics);
        ArgumentNullException.ThrowIfNull(dataTransfer);
        ArgumentNullException.ThrowIfNull(imports);
        ArgumentNullException.ThrowIfNull(users);
        ArgumentNullException.ThrowIfNull(auth);
        ArgumentNullException.ThrowIfNull(health);
        ArgumentNullException.ThrowIfNull(meta);
        
        Activities = activities;
        Records = records;
        Tags = tags;
        Events = events;
        Statistics = statistics;
        DataTransfer = dataTransfer;
        Imports = imports;
        Users = users;
        Auth = auth;
        Health = health;
        Meta = meta;
    }
}