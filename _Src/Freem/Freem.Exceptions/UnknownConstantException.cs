namespace Freem.Exceptions;

public sealed class UnknownConstantException : Exception
{
    public string Value { get; }

    public UnknownConstantException(string value)
        : base($"Unknown constant \"{value}\"")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }
}
