using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.Abstractions.Exceptions.Extensions;

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
        return $"Entity with id = {id.ToQuotedString()} not found";
    }
}
