using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.DataTransfer.Import;

public sealed class ImportActivityId : Id, IParsable<ImportActivityId>
{
    private ImportActivityId(string value)
        : base(value)
    {
    }

    public static ImportActivityId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for ImportActivityId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportActivityId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new ImportActivityId(s);
        return true;
    }
}
