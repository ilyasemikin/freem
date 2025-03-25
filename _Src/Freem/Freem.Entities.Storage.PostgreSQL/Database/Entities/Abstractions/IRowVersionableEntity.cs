namespace Freem.Entities.Storage.PostgreSQL.Database.Entities.Abstractions;

internal interface IRowVersionableEntity
{
    public uint RowVersion { get; }
}
