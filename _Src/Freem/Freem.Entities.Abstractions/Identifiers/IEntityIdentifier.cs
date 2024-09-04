namespace Freem.Entities.Abstractions.Identifiers;

public interface IEntityIdentifier : IEquatable<IEntityIdentifier>
{
    string Value { get; }
}
