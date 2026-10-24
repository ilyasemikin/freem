using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.DataTransfer.Import;

public sealed class ImportTagId : Id, IParsable<ImportTagId>
{
    private ImportTagId(string value)
        : base(value)
    {
    }

    public static ImportTagId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for ImportTagId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportTagId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new ImportTagId(s);
        return true;
    }
}
