using Freem.Domain.Events;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal static class EntityTypeNames
{
    public static string ToName(EntityType type)
    {
        return type switch
        {
            EntityType.Activity => EntityNames.Activity,
            EntityType.Record => EntityNames.Record,
            EntityType.RunningRecord => EntityNames.RunningRecord,
            EntityType.Tag => EntityNames.Tag,
            EntityType.User => EntityNames.User,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static EntityType Parse(string name)
    {
        return name switch
        {
            EntityNames.Activity => EntityType.Activity,
            EntityNames.Record => EntityType.Record,
            EntityNames.RunningRecord => EntityType.RunningRecord,
            EntityNames.Tag => EntityType.Tag,
            EntityNames.User => EntityType.User,
            _ => throw new ArgumentOutOfRangeException(nameof(name), name, null)
        };
    }
}
