using Freem.Entities.UseCases.Contracts;

namespace Freem.Web.Api.Public.Mappers;

internal static class UpdateFieldMapper
{
    public static Freem.Entities.UseCases.Contracts.UpdateField<T> Map<T>(this Contracts.UpdateField<T> field)
    {
        return new UpdateField<T>(field.Value);
    }
}