using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Import;

public sealed class ImportDocumentStatus : IEquatable<ImportDocumentStatus>, IParsable<ImportDocumentStatus>
{
    private readonly Value _value;

    public static ImportDocumentStatus Created { get; } = new(Value.Created);
    public static ImportDocumentStatus Parsing { get; } = new(Value.Parsing);
    public static ImportDocumentStatus Parsed { get; } = new(Value.Parsed);
    public static ImportDocumentStatus Failed { get; } = new(Value.Failed);

    public ImportDocumentStatus(Value value)
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, null);

        _value = value;
    }

    public bool Equals(ImportDocumentStatus? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is ImportDocumentStatus other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)_value;
    }

    public override string ToString()
    {
        return _value.ToString("G");
    }

    public static bool operator ==(ImportDocumentStatus left, ImportDocumentStatus right)
    {
        return left._value == right._value;
    }

    public static bool operator !=(ImportDocumentStatus left, ImportDocumentStatus right)
    {
        return !(left == right);
    }

    public bool CanChangeTo(ImportDocumentStatus next)
    {
        ArgumentNullException.ThrowIfNull(next);

        return CanChangeTo(next._value);
    }

    private bool CanChangeTo(Value next)
    {
        return _value switch
        {
            Value.Created => next == Value.Parsing,
            Value.Parsing => next == Value.Parsed || next == Value.Failed,
            _ => false
        };
    }

    public Value ToValue()
    {
        return _value;
    }

    public static ImportDocumentStatus Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result)
            ? result
            : throw new FormatException($"Invalid value for ImportDocumentStatus: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportDocumentStatus? result)
    {
        if (!Enum.TryParse<Value>(s, out var value) || !Enum.IsDefined(value))
        {
            result = null;
            return false;
        }

        result = new ImportDocumentStatus(value);
        return true;
    }

    public enum Value
    {
        Created,
        Parsing,
        Parsed,
        Failed
    }
}
