namespace Freem.Domain;

public interface IEntity<TEntityId>
    where TEntityId : Id
{
    TEntityId Id { get; }
}
