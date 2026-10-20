using Freem.Domain.UseCases.Contracts;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class UpdateFieldMapper
{
    public static UpdateField<T> Map<T>(this Freem.Api.Public.Contracts.Models.UpdateField<T> field)
    {
        return new UpdateField<T>(field.Value);
    }
}
