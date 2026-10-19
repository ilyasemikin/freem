using System.Diagnostics.CodeAnalysis;

namespace Freem.Domain.Users;

public sealed class UserId : Id, IParsable<UserId>
{
    private UserId(string value)
        : base(value)
    {
    }

    public static UserId Generate()
    {
        var value = GenerateValue();
        return new UserId(value);
    }

    public static UserId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for UserId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out UserId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new UserId(s);
        return true;
    }
}
