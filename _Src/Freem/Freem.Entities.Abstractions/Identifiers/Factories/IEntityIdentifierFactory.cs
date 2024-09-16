namespace Freem.Entities.Abstractions.Identifiers.Factories;

public interface IEntityIdentifierFactory<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    TEntityIdentifier Create();
}