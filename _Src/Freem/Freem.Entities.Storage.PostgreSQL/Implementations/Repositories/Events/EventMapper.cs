using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using EventActionDomain = Freem.Entities.Abstractions.EventAction;
using EventActionDb = Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base.EventAction;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

internal static class EventMapper
{
    public static IEventEntity<EventIdentifier, UserIdentifier> MapToDomainEntity(this BaseEventEntity entity)
    {
        return entity switch
        {
            CategoryEventEntity category => CategoryEventMapper.MapToDomainEntity(category),
            RecordEventEntity record => RecordEventMapper.MapToDomainEntity(record),
            RunningRecordEventEntity runningRecord => RunningRecordEventMapper.MapToDomainEntity(runningRecord),
            TagEventEntity tag => TagEventMapper.MapToDomainEntity(tag),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static EventActionDb MapToDatabaseModel(this EventActionDomain action)
    {
        return action switch
        {
            EventActionDomain.Created => EventActionDb.Created,
            EventActionDomain.Updated => EventActionDb.Updated,
            EventActionDomain.Removed => EventActionDb.Removed,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static EventActionDomain MapToDomainModel(this EventActionDb action)
    {
        return action switch
        {
            EventActionDb.Created => EventActionDomain.Created,
            EventActionDb.Updated => EventActionDomain.Updated,
            EventActionDb.Removed => EventActionDomain.Removed,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}