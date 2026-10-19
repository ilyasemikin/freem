using Freem.Domain.Identifiers;
using Freem.Domain.Storage.Exceptions.Base;

namespace Freem.Domain.Storage.Exceptions;

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
