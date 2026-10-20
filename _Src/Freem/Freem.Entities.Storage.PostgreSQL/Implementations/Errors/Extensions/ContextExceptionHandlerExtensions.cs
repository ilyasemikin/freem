using Freem.Entities.Storage.PostgreSQL.Database;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Errors.Extensions;

internal static class ContextExceptionHandlerExtensions
{
    public static async Task<Result<Error>> HandleSaveChangesAsync(
        this DatabaseContextWriteExceptionHandler handler, 
        DatabaseContextWriteContext context,
        DatabaseContext database, 
        CancellationToken cancellationToken)
    {
        return await handler.Handle(context, () => database.SaveChangesAsync(cancellationToken));
    }
}