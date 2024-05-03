namespace Freem.Collections.Identifiers.Abstractions;

public interface IReadOnlyIdentifiersCollection : IReadOnlyCollection<string>
{
    bool Empty { get; }

    bool Contains(string identifier);
}
