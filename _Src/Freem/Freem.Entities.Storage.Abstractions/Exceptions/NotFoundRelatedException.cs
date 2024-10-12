using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Identifiers.Abstractions;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class NotFoundRelatedException : StorageException
{
    public IIdentifier Id { get; }
    public IReadOnlyList<IIdentifier> RelatedIds { get; }

    public NotFoundRelatedException(IIdentifier id, IIdentifier relatedId)
        : base(GenerateMessage(relatedId))
    {
        Id = id;
        RelatedIds = [relatedId];
    }
    
    public NotFoundRelatedException(IIdentifier id, params IIdentifier[] relatedIds)
        : base(GenerateMessage(relatedIds))
    {
        Id = id;
        RelatedIds = relatedIds;
    }

    private static string GenerateMessage(IIdentifier relatedId)
    {
        return $"Related id = \"{relatedId}\" not found";
    }

    private static string GenerateMessage(IReadOnlyList<IIdentifier> relatedIds)
    {
        return relatedIds.Count switch
        {
            0 => $"Related ids not found",
            1 => GenerateMessage(relatedIds[0]),
            _ => $"Related with ids = [{string.Join(", ", relatedIds.Select(id => $"\"{id}\""))}]"
        };
    }
}
