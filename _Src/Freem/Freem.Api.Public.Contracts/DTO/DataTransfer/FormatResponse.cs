using Freem.Domain.Formats;

namespace Freem.Api.Public.Contracts.DTO.DataTransfer;

/// <summary>A data format supported for export or import.</summary>
public sealed class FormatResponse
{
    /// <summary>Format key, used to select the format in requests.</summary>
    public FormatKey Key { get; }

    /// <summary>Human-readable format name.</summary>
    public FormatName Name { get; }

    /// <summary>File extension without a leading dot.</summary>
    public FormatFileExtension FileExtension { get; }

    /// <summary>Media type of the file.</summary>
    public FormatMediaType MediaType { get; }

    public FormatResponse(
        FormatKey key,
        FormatName name,
        FormatFileExtension fileExtension,
        FormatMediaType mediaType)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(fileExtension);
        ArgumentNullException.ThrowIfNull(mediaType);

        Key = key;
        Name = name;
        FileExtension = fileExtension;
        MediaType = mediaType;
    }
}
