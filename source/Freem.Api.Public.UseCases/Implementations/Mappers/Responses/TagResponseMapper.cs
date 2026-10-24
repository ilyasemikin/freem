using Freem.Api.Public.Contracts.V1.Tags;
using Freem.Api.Public.Contracts.V1.Tags.Dtos;
using Freem.Domain;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.Contracts.Tags.Get;
using Freem.Domain.UseCases.Contracts.Tags.Search;
using Freem.Domain.UseCases.Contracts.Tags.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiTagCreateResponse = Freem.Api.Public.Contracts.V1.Tags.TagCreateResponse;
using TagDto = Freem.Api.Public.Contracts.V1.Tags.Dtos.TagDto;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class TagResponseMapper
{
    public static IActionResult Map(this Result<CreateTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiTagCreateResponse(result.Value.Tag.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<SearchTagByNameResponse, Error> result, TagName name)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tag = result.Value.Tags.FirstOrDefault();
        if (tag is null)
        {
            var error = DomainErrorCatalog.Factories.CreateTagNotFound(name);
            return error.MapError();
        }

        var dto = tag.Map();
        var response = new TagResponse(dto);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tag = result.Value.Tag.Map();
        var response = new TagResponse(tag);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tags = result.Value.Tags
            .Select(tag => tag.Map())
            .ToArray();
        var response = new TagListResponse(result.Value.TotalCount, tags);
        return new OkObjectResult(response);
    }
}
