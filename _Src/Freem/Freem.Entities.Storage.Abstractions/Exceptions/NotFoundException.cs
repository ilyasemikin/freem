using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions.Base;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class NotFoundException : StorageException
{
    public IEntityIdentifier Id { get; }

    public NotFoundException(IEntityIdentifier id)
        : base(GenerateMessage(id))
    {
        Id = id;
    }

    private static string GenerateMessage(IEntityIdentifier id)
    {
        return $"Entity with id = \"{id}\" not found";
    }
}
