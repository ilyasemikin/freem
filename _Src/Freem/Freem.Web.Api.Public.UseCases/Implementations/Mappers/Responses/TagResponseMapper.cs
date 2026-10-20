using Freem.Domain;
using Freem.Domain.Tags.Models;
using Freem.Entities.UseCases.Contracts.Tags.Create;
using Freem.Entities.UseCases.Contracts.Tags.Get;
using Freem.Entities.UseCases.Contracts.Tags.GetByName;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Web.Api.Public.UseCases.Implementations.Results;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiCreateTagResponse = Freem.Web.Api.Public.Contracts.DTO.Tags.CreateTagResponse;
using TagResponse = Freem.Web.Api.Public.Contracts.DTO.Tags.TagResponse;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;

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
