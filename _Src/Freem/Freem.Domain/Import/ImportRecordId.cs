using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Import;

public sealed class ImportRecordId : Id, IParsable<ImportRecordId>
{
    private ImportRecordId(string value)
        : base(value)
    {
    }

    public static ImportRecordId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for ImportRecordId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportRecordId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new ImportRecordId(s);
        return true;
    }
}
