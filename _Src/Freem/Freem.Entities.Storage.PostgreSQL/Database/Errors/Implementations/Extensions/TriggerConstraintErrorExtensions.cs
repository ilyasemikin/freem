using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.RunningRecords.Identifiers;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations.Extensions;

internal static class TriggerConstraintErrorExtensions
{
    public static string AsString(this TriggerConstraintError.Parameter parameter)
    {
        return parameter.Value;
    }
    
    public static int AsInt(this TriggerConstraintError.Parameter parameter)
    {
        return Convert.ToInt32(parameter.Value);
    }

    public static ActivityIdentifier AsActivityIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return parameter.Value;
    }

    public static RecordIdentifier AsRecordIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return parameter.Value;
    }

    public static RunningRecordIdentifier AsRunningRecordIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return parameter.Value;
    }
    
    public static TagIdentifier AsTagIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return parameter.Value;
    }

    public static UserIdentifier AsUserIdentifier(this TriggerConstraintError.Parameter parameter)
    {
        return parameter.Value;
    }
}