namespace Freem.Entities.Abstractions.Identifiers.Factories;

public interface IEventIdentifierFactory<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    TEntityIdentifier Create();
}