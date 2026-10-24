using Freem.Domain.Users.Models;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

public sealed class ImportParseOptions
{
    public TimeZoneOffset TimeZoneOffset { get; }

    public ImportParseOptions(TimeZoneOffset timeZoneOffset)
    {
        ArgumentNullException.ThrowIfNull(timeZoneOffset);

        TimeZoneOffset = timeZoneOffset;
    }
}
