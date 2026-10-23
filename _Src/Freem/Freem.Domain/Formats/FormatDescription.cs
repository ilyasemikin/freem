namespace Freem.Domain.Formats;

public sealed class FormatDescription
{
    public FormatKey Key { get; }
    public FormatName Name { get; }
    public FormatFileExtension FileExtension { get; }
    public FormatMediaType MediaType { get; }

    public FormatDescription(
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
