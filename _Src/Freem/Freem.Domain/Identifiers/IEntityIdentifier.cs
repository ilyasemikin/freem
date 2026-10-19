using Freem.Identifiers.Abstractions;

namespace Freem.Domain.Identifiers;

public interface IEntityIdentifier : IIdentifier, IEquatable<IEntityIdentifier>
{
    string ToString();
}
