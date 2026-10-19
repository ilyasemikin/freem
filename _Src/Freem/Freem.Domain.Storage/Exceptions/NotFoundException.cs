using Freem.Domain.Storage.Exceptions.Base;

namespace Freem.Domain.Storage.Exceptions;

public sealed class NotFoundException : StorageException
{
    public Id Id { get; }

    public NotFoundException(Id id)
        : base(GenerateMessage(id))
    {
        Id = id;
    }

    private static string GenerateMessage(Id id)
    {
        return $"Entity with id = \"{id}\" not found";
    }
}
