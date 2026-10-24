using Freem.Domain.UseCases.Contracts;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class UpdateFieldMapper
{
    public static UpdateField<T> Map<T>(this Public.Contracts.V1.UpdateField<T> field)
    {
        return new UpdateField<T>(field.Value);
    }
}
