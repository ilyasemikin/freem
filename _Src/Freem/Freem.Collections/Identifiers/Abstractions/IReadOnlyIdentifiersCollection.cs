namespace Freem.Collections.Identifiers.Abstractions;

public interface IReadOnlyIdentifiersCollection : IReadOnlyCollection<string>
{
    bool Contains(string identifier);
}
