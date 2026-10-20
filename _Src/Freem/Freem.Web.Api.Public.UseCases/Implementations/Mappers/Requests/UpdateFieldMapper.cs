using Freem.Entities.UseCases.Contracts;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class UpdateFieldMapper
{
    public static UpdateField<T> Map<T>(this Freem.Web.Api.Public.Contracts.Models.UpdateField<T> field)
    {
        return new UpdateField<T>(field.Value);
    }
}
