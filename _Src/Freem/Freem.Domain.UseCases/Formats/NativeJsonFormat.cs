using Freem.Domain.Formats;

namespace Freem.Domain.UseCases.Formats;

public static class NativeJsonFormat
{
    public const string FormatMarker = "freem";
    public const int CurrentVersion = 1;

    public static FormatDescription Description { get; } = CreateDescription();

    private static FormatDescription CreateDescription()
    {
        var key = FormatKey.Parse("freem-json", null);
        var name = FormatName.Parse("Freem JSON", null);
        var fileExtension = FormatFileExtension.Parse("json", null);
        var mediaType = FormatMediaType.Parse("application/json", null);
        return new FormatDescription(key, name, fileExtension, mediaType);
    }

    public static class Properties
    {
        public const string Format = "format";
        public const string Version = "version";
        public const string Tags = "tags";
        public const string Activities = "activities";
        public const string Records = "records";

        public const string Id = "id";
        public const string Name = "name";
        public const string Description = "description";
        public const string Color = "color";
        public const string Status = "status";
        public const string TagIds = "tag_ids";
        public const string ActivityIds = "activity_ids";
        public const string Periods = "periods";
        public const string StartAt = "start_at";
        public const string EndAt = "end_at";
        public const string TimeZoneOffset = "time_zone_offset";
    }
}
