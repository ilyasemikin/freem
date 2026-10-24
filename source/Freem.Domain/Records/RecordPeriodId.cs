using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Records;

public sealed class RecordPeriodId : Id, IParsable<RecordPeriodId>
{
    private RecordPeriodId(string value)
        : base(value)
    {
    }

    public static RecordPeriodId Generate()
    {
        var value = GenerateValue();
        return new RecordPeriodId(value);
    }

    public static RecordPeriodId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for RecordPeriodId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out RecordPeriodId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new RecordPeriodId(s);
        return true;
    }
}
