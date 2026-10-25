using Freem.Domain.DataTransfer.Formats;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Formats.TimeTracker;

public static class TimeTrackerFormat
{
    public static FormatDescription Description { get; } = CreateDescription();

    private static FormatDescription CreateDescription()
    {
        var key = FormatKey.Parse("time-tracker", null);
        var name = FormatName.Parse("Time Tracker (CSV)", null);
        var fileExtension = FormatFileExtension.Parse("csv", null);
        var mediaType = FormatMediaType.Parse("text/csv", null);
        return new FormatDescription(key, name, fileExtension, mediaType);
    }
}
