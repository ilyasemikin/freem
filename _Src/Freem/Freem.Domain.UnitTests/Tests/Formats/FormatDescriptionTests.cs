using Freem.Domain.Formats;

namespace Freem.Domain.UnitTests.Tests.Formats;

public sealed class FormatDescriptionTests
{
    [Fact]
    public void Constructor_ShouldKeepValues()
    {
        var key = FormatKey.Parse("freem-json", null);
        var name = FormatName.Parse("Freem JSON", null);
        var fileExtension = FormatFileExtension.Parse("json", null);
        var mediaType = FormatMediaType.Parse("application/json", null);

        var description = new FormatDescription(key, name, fileExtension, mediaType);

        Assert.Equal(key, description.Key);
        Assert.Equal(name, description.Name);
        Assert.Equal(fileExtension, description.FileExtension);
        Assert.Equal(mediaType, description.MediaType);
    }
}
