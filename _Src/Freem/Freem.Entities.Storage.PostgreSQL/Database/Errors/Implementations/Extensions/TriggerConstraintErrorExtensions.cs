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
}