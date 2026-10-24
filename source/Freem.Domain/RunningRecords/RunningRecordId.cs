using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Users;

namespace Freem.Domain.RunningRecords;

public sealed class RunningRecordId : Id, IParsable<RunningRecordId>
{
    private RunningRecordId(string value)
        : base(value)
    {
    }

    public static RunningRecordId FromUserId(UserId userId)
    {
        var value = userId.ToString();
        return new RunningRecordId(value);
    }

    public UserId ToUserId()
    {
        var value = ToString();
        return UserId.Parse(value, null);
    }

    public static RunningRecordId Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var id)
            ? id
            : throw new FormatException($"Invalid value for RunningRecordId: {s}");
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s,
        IFormatProvider? provider,
        [NotNullWhen(true)] out RunningRecordId? result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = null;
            return false;
        }

        result = new RunningRecordId(s);
        return true;
    }
}
