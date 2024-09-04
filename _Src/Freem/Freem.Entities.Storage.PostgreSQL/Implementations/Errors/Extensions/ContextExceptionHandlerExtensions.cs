using Freem.Entities.Storage.PostgreSQL.Database;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;

internal static class ContextExceptionHandlerExtensions
{
    public static async Task HandleSaveChangesAsync(
        this DatabaseContextExceptionHandler handler, 
        DatabaseContext database, 
        CancellationToken cancellationToken)
    {
        await handler.Handle(() => database.SaveChangesAsync(cancellationToken));
    }
}