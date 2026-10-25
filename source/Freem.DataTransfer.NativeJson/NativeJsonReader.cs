using System.Text.Json;
using Freem.DataTransfer.NativeJson.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.DataTransfer.NativeJson;

public sealed class NativeJsonReader
{
    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

    public async Task<Result<NativeJsonFile, Error>> ReadAsync(
        Stream input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        NativeJsonFile? file;
        try
        {
            file = await JsonSerializer.DeserializeAsync<NativeJsonFile>(input, SerializerOptions, cancellationToken);
        }
        catch (JsonException exception)
        {
            return NativeJsonErrorCatalog.Factories.CreateInvalidJson(exception.Message);
        }

        if (file is null)
            return NativeJsonErrorCatalog.Factories.CreateEmptyFile();
        if (file.Format != NativeJsonSchema.FormatMarker)
            return NativeJsonErrorCatalog.Factories.CreateUnexpectedFormat(file.Format);
        if (file.Version != NativeJsonSchema.CurrentVersion)
            return NativeJsonErrorCatalog.Factories.CreateUnsupportedVersion(file.Version);

        return file;
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }
}
