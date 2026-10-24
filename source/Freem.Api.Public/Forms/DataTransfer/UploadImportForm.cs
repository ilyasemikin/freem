using System.ComponentModel.DataAnnotations;

namespace Freem.Api.Public.Forms.DataTransfer;

/// <summary>Multipart form for uploading a file to import.</summary>
public sealed class UploadImportForm
{
    /// <summary>Key of the format the file is in, see the list of formats.</summary>
    [Required]
    public string Format { get; init; } = string.Empty;

    /// <summary>Time zone offset the file times are in, for example <c>03:00:00</c>. The user's setting is used when omitted.</summary>
    public string? TimeZoneOffset { get; init; }

    /// <summary>The file to import.</summary>
    [Required]
    public IFormFile File { get; init; } = null!;
}
