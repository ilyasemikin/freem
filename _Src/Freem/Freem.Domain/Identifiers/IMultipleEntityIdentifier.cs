namespace Freem.Domain.Identifiers;

public interface IMultipleEntityIdentifier
{
    IEnumerable<IEntityIdentifier> Ids { get; }
}