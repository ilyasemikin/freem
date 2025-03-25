using Microsoft.EntityFrameworkCore;

namespace Freem.EFCore.Extensions;

public static class ModelBuilderExtensions 
{
    public static void ApplyConfiguration<TEntity, TEntityConfiguration>(this ModelBuilder builder)
        where TEntityConfiguration : IEntityTypeConfiguration<TEntity>, new()
        where TEntity : class
    {
        var configuration = new TEntityConfiguration();
        builder.ApplyConfiguration(configuration);
    }
}