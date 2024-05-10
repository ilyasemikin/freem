namespace Freem.Entities.Storage.PostgreSQL.Database.Entities;

internal class TagEntity
{
    public required string Id { get; init; }

    public required string Name { get; set; }
}
