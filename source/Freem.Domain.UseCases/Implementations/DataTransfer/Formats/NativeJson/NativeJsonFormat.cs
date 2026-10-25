using Freem.Domain.DataTransfer.Formats;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Formats.NativeJson;

public static class NativeJsonFormat
{
    public static FormatDescription Description { get; } = CreateDescription();

    private static FormatDescription CreateDescription()
    {
        var key = FormatKey.Parse("freem-json", null);
        var name = FormatName.Parse("Freem JSON", null);
        var fileExtension = FormatFileExtension.Parse("json", null);
        var mediaType = FormatMediaType.Parse("application/json", null);
        return new FormatDescription(key, name, fileExtension, mediaType);
    }
}
