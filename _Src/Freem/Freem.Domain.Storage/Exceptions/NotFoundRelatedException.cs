using Freem.Domain.Storage.Exceptions.Base;

namespace Freem.Domain.Storage.Exceptions;

public sealed class NotFoundRelatedException : StorageException
{
    public Id Id { get; }
    public IReadOnlyList<Id> RelatedIds { get; }

    public NotFoundRelatedException(Id id, Id relatedId)
        : base(GenerateMessage(relatedId))
    {
        Id = id;
        RelatedIds = [relatedId];
    }
    
    public NotFoundRelatedException(Id id, params Id[] relatedIds)
        : base(GenerateMessage(relatedIds))
    {
        Id = id;
        RelatedIds = relatedIds;
    }

    private static string GenerateMessage(Id relatedId)
    {
        return $"Related id = \"{relatedId}\" not found";
    }

    private static string GenerateMessage(IReadOnlyList<Id> relatedIds)
    {
        return relatedIds.Count switch
        {
            0 => $"Related ids not found",
            1 => GenerateMessage(relatedIds[0]),
            _ => $"Related with ids = [{string.Join(", ", relatedIds.Select(id => $"\"{id}\""))}]"
        };
    }
}
