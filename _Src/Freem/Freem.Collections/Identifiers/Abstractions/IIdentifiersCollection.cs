namespace Freem.Collections.Identifiers.Abstractions;

public interface IIdentifiersCollection : IReadOnlyIdentifiersCollection
{   
    void Add(string identifier);
    void AddRange(IEnumerable<string> identifiers);
    void Remove(string identifier);
    void Update(IEnumerable<string> identifiers);
    void Clear();
}
