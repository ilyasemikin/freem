namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal class UserEntity
{
    public required string Id { get; init; }

    public IList<CategoryEntity>? Categories { get; set; }
}
