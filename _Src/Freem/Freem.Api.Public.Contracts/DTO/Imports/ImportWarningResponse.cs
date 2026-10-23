namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>A note about a file row that was skipped or changed while parsing.</summary>
public sealed class ImportWarningResponse
{
    /// <summary>One-based line number in the uploaded file.</summary>
    public int LineNumber { get; }

    /// <summary>What happened to the row.</summary>
    public string Message { get; }

    public ImportWarningResponse(int lineNumber, string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        LineNumber = lineNumber;
        Message = message;
    }
}
