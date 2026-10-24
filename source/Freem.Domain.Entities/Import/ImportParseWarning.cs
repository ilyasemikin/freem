using Utilities.Errors;

namespace Freem.Domain.Entities.Import;

public sealed class ImportParseWarning
{
    public int LineNumber { get; }
    public ErrorMessage Message { get; }

    public ImportParseWarning(int lineNumber, ErrorMessage message)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(lineNumber);
        ArgumentNullException.ThrowIfNull(message);

        LineNumber = lineNumber;
        Message = message;
    }
}
