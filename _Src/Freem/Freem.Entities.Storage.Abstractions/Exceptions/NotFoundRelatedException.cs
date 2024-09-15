using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.Abstractions.Exceptions.Extensions;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class NotFoundRelatedException : StorageException
{
    public IEntityIdentifier Id { get; }
    public IReadOnlyList<IEntityIdentifier> RelatedIds { get; }

    public NotFoundRelatedException(IEntityIdentifier id, IEntityIdentifier relatedId)
        : base(GenerateMessage(relatedId))
    {
        Id = id;
        RelatedIds = [relatedId];
    }
    
    public NotFoundRelatedException(IEntityIdentifier id, params IEntityIdentifier[] relatedIds)
        : base(GenerateMessage(relatedIds))
    {
        Id = id;
        RelatedIds = relatedIds;
    }

    private static string GenerateMessage(IEntityIdentifier relatedId)
    {
        return $"Related id = {relatedId.ToQuotedString()} not found";
    }

    private static string GenerateMessage(IReadOnlyList<IEntityIdentifier> relatedIds)
    {
        return relatedIds.Count switch
        {
            0 => $"Related ids not found",
            1 => GenerateMessage(relatedIds[0]),
            _ => $"Related with ids = {relatedIds.ToListString()}"
        };
    }
}
