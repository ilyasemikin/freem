namespace Freem.Api.Public.Contracts.V1.Imports.Dtos;

/// <summary>A note about a file row that was skipped or changed while parsing.</summary>
public sealed class ImportWarningDto
{
    /// <summary>One-based line number in the uploaded file.</summary>
    public int LineNumber { get; }

    /// <summary>What happened to the row.</summary>
    public string Message { get; }

    public ImportWarningDto(int lineNumber, string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        LineNumber = lineNumber;
        Message = message;
    }
}
