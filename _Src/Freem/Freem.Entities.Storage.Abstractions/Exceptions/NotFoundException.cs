using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.Abstractions.Exceptions.Extensions;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class NotFoundException : StorageException
{
    public IReadOnlyList<IEntityIdentifier> IdValues { get; }

    public NotFoundException(IEntityIdentifier id)
        : base(GenerateMessage(id))
    {
        IdValues = [id];
    }

    public NotFoundException(params IEntityIdentifier[] ids)
        : base(GenerateMessage(ids))
    {
        IdValues = ids;
    }

    private static string GenerateMessage(IEntityIdentifier id)
    {
        return $"Entity with id = {id.ToQuotedString()} not found";
    }

    private static string GenerateMessage(IReadOnlyList<IEntityIdentifier> ids)
    {
        return ids.Count switch
        {
            0 => $"Entity not found",
            1 => GenerateMessage(ids[0]),
            _ => $"Entities {ids.ToListString()} not found"
        };
    }
}
