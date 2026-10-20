using Freem.Domain;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Contracts.Tags.Create;
using Freem.Domain.UseCases.Contracts.Tags.Get;
using Freem.Domain.UseCases.Contracts.Tags.GetByName;
using Freem.Domain.UseCases.Contracts.Tags.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Api.Public.UseCases.Implementations.Results;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiCreateTagResponse = Freem.Api.Public.Contracts.DTO.Tags.CreateTagResponse;
using TagResponse = Freem.Api.Public.Contracts.DTO.Tags.TagResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class TagResponseMapper
{
    public static IActionResult Map(this Result<CreateTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiCreateTagResponse(result.Value.Tag.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<FindTagByNameResponse, Error> result, TagName name)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tag = result.Value.Tags.FirstOrDefault();
        if (tag is null)
        {
            var error = DomainErrorCatalog.Factories.CreateTagNotFound(name);
            return error.MapError();
        }

        var response = tag.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = result.Value.Tag.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListTagResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var tags = result.Value.Tags
            .Select(tag => tag.Map())
            .ToArray();
        return new ItemsListResult<TagResponse>(tags, result.Value.TotalCount);
    }
}
