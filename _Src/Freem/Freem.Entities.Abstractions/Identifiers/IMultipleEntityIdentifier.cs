namespace Freem.Entities.Abstractions.Identifiers;

public interface IMultipleEntityIdentifier
{
    IEnumerable<IEntityIdentifier> Ids { get; }
}