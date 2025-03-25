using Microsoft.EntityFrameworkCore;

namespace Freem.EFCore.Extensions;

public static class DbContextExtensions
{
    public static void DetachEntry<T>(this DbContext context, T entity) 
        where T : class
    {
        context.Entry(entity).State = EntityState.Detached;
    }
}