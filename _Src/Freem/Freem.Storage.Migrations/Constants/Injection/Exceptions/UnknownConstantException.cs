namespace Freem.Storage.Migrations.Constants.Injection.Exceptions;

public sealed class UnknownConstantException : Exception
{
    public string Name { get; }
    public int Index { get; }
    public int Count { get; }
    
    public UnknownConstantException(string name, int index, int count)
        : base($"Unknown constant \"{name}\" at position \"{index}\"")
    {
        Name = name;
        Index = index;
        Count = count;
    }
}