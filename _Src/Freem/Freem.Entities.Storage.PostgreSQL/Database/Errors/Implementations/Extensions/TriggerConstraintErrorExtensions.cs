using Freem.Entities.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;

internal static class TriggerConstraintErrorExtensions
{
    public static int AsInt(this TriggerConstraintError.Parameter parameter)
    {
        return Convert.ToInt32(parameter.Value);
    }

    public static ActivityIdentifier AsActivityIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return new ActivityIdentifier(parameter.Value);
    }

    public static RecordIdentifier AsRecordIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return new RecordIdentifier(parameter.Value);
    }
    
    public static TagIdentifier AsTagIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return new TagIdentifier(parameter.Value);
    }

    public static UserIdentifier AsUserIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return new UserIdentifier(parameter.Value);
    }
}