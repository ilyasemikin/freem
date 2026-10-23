using Freem.Api.Public.Contracts.DTO.DataTransfer;
using Freem.Domain.Formats;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class FormatMapper
{
    public static FormatResponse Map(this FormatDescription format)
    {
        return new FormatResponse(format.Key, format.Name, format.FileExtension, format.MediaType);
    }
}
