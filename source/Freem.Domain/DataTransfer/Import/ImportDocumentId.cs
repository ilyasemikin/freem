using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.DataTransfer.Import;

public sealed class ImportDocumentId : Id, IParsable<ImportDocumentId>
{
    private ImportDocumentId(string value)
        : base(value)
    {
    }

    public static ImportDocumentId Generate()
    {
        var value = GenerateValue();
        return new ImportDocumentId(value);
    }

    public static ImportDocumentId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for ImportDocumentId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out ImportDocumentId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new ImportDocumentId(s);
        return true;
    }
}
