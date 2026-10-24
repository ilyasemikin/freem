using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Tags;

public sealed class TagId : Id, IParsable<TagId>
{
    private TagId(string value)
        : base(value)
    {
    }

    public static TagId Generate()
    {
        var value = GenerateValue();
        return new TagId(value);
    }

    public static TagId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for TagId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out TagId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new TagId(s);
        return true;
    }
}
