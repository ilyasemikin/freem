using Freem.Domain.Entities.Tags;
using Freem.Api.Public.Contracts.DTO.Tags;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class TagMapper
{
    public static TagResponse Map(this Tag tag)
    {
        return new TagResponse(tag.Id, tag.Name);
    }
}
