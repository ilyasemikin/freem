using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions.Base;
using Freem.Entities.Storage.Abstractions.Exceptions.Extensions;

namespace Freem.Entities.Storage.Abstractions.Exceptions;

public sealed class NotFoundRelatedException : StorageException
{
    public IEntityIdentifier Id { get; }

    public string RelatedName { get; }
    public IReadOnlyList<IEntityIdentifier> RelatedIds { get; }

    public NotFoundRelatedException(IEntityIdentifier id, string relatedName, IEntityIdentifier relatedId)
        : base(GenerateMessage(id, relatedName, relatedId))
    {
        Id = id;
        RelatedName = relatedName;
        RelatedIds = [relatedId];
    }

    public NotFoundRelatedException(IEntityIdentifier id, string relatedName, params IEntityIdentifier[] relatedIds)
        : base(GenerateMessage(id, relatedName, relatedIds))
    {
        Id = id;
        RelatedName = relatedName;
        RelatedIds = relatedIds;
    }

    private static string GenerateMessage(IEntityIdentifier id, string relatedName, IEntityIdentifier relatedId)
    {
        return $"Related {relatedName.ToQuotedString()} with id = {relatedId.ToQuotedString()} not found for id = {id.ToQuotedString()}";
    }

    private static string GenerateMessage(IEntityIdentifier id, string relatedName, IReadOnlyList<IEntityIdentifier> relatedIds)
    {
        return relatedIds.Count switch
        {
            0 => $"Related {relatedName.ToQuotedString()} ids not found for id = {id.ToQuotedString()}",
            1 => GenerateMessage(id, relatedName, relatedIds[0]),
            _ => $"Related {relatedName.ToQuotedString()} with ids = {relatedIds.ToListString()} for id = {id.ToQuotedString()}"
        };
    }
}
