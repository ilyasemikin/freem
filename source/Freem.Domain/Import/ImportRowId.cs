using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Import;

public sealed class ImportRowId : Id, IParsable<ImportRowId>
{
    private ImportRowId(string value)
        : base(value)
    {
    }

    public static ImportRowId Generate()
    {
        var value = GenerateValue();
        return new ImportRowId(value);
    }

    public static ImportRowId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for ImportRowId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportRowId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new ImportRowId(s);
        return true;
    }
}
