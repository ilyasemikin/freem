using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Records;

public sealed class RecordId : Id, IParsable<RecordId>
{
    private RecordId(string value)
        : base(value)
    {
    }

    public static RecordId Generate()
    {
        var value = GenerateValue();
        return new RecordId(value);
    }

    public static RecordId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for RecordId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out RecordId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new RecordId(s);
        return true;
    }
}
