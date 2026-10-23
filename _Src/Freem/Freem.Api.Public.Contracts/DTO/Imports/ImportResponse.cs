using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Users.Models;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>An import: an uploaded and parsed file whose rows can be reviewed.</summary>
public sealed class ImportResponse
{
    /// <summary>Import id.</summary>
    public ImportDocumentId Id { get; }

    /// <summary>Key of the format the file was parsed with.</summary>
    public FormatKey Format { get; }

    /// <summary>Name of the uploaded file.</summary>
    public ImportFileName FileName { get; }

    /// <summary>Time zone offset used to read the file times.</summary>
    public TimeZoneOffset TimeZoneOffset { get; }

    /// <summary>Import status.</summary>
    public ImportDocumentStatus Status { get; }

    /// <summary>Number of rows in the uploaded file.</summary>
    public int RowsInFile { get; }

    /// <summary>Number of rows that were parsed and saved.</summary>
    public int RowsParsed { get; }

    /// <summary>Tags found in the file.</summary>
    public IReadOnlyList<ImportTagResponse> Tags { get; }

    /// <summary>Activities found in the file.</summary>
    public IReadOnlyList<ImportActivityResponse> Activities { get; }

    public ImportResponse(
        ImportDocumentId id,
        FormatKey format,
        ImportFileName fileName,
        TimeZoneOffset timeZoneOffset,
        ImportDocumentStatus status,
        int rowsInFile,
        int rowsParsed,
        IReadOnlyList<ImportTagResponse> tags,
        IReadOnlyList<ImportActivityResponse> activities)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(format);
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(timeZoneOffset);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(activities);

        Id = id;
        Format = format;
        FileName = fileName;
        TimeZoneOffset = timeZoneOffset;
        Status = status;
        RowsInFile = rowsInFile;
        RowsParsed = rowsParsed;
        Tags = tags;
        Activities = activities;
    }
}
