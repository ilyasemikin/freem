using Freem.Api.Public.Contracts.V1.Tags.Dtos;
using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Domain.Entities.Tags;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class TagMapper
{
    public static TagDto Map(this Tag tag)
    {
        return new TagDto(tag.Id, tag.Name);
    }
}
