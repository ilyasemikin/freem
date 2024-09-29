using System.Text.Json;
using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;
using Freem.Exceptions;
using Freem.Reflection.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal class EventEntityToDatabaseEntityConverter 
    : IConverter<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventEntity>
{
    public EventEntity Convert(IEntityEvent<IEntityIdentifier, UserIdentifier> entity)
    {
        return new EventEntity
        {
            Id = entity.Id,
            EntityId = entity.EntityId.ToString(),
            EntityName = GetEntityName(entity.EntityId),
            UserId = entity.UserEntityId,
            Action = entity.Action,
            AdditionalData = GetJsonData(entity)
        };
    }

    private static string GetEntityName(IEntityIdentifier id)
    {
        return id switch
        {
            ActivityIdentifier => EntitiesNames.Activities.EntityName,
            RecordIdentifier => EntitiesNames.Records.EntityName,
            RunningRecordIdentifier => EntitiesNames.RunningRecords.EntityName,
            TagIdentifier => EntitiesNames.Tags.EntityName,
            UserIdentifier => EntitiesNames.Users.EntityName,
            _ => throw new UnknownConstantException(id.GetType().Name)
        };
    }

    private static string? GetJsonData(IEntityEvent<IEntityIdentifier, UserIdentifier> entity)
    {
        const string interfaceName = "IEntityEvent`3";
        
        var type = entity.GetType();
        if (!type.TryGetInterface(interfaceName, out _)) 
            return null;
        
        var property = type.GetProperty("Data");
        var data = property?.GetValue(entity);
        return data is not null ? JsonSerializer.Serialize(data) : null;
    }
}