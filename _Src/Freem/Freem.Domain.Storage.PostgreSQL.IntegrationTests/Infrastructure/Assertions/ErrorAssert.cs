using Freem.Domain;
using Utilities.Errors;
using Xunit;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;

internal static class ErrorAssert
{
    public static void EntityNotFound(Error? error, Id id)
    {
        Assert.NotNull(error);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, error.Type);
        AssertIdProperty(error, id);
    }

    public static void RelatedEntitiesNotFound(Error? error, params Id[] ids)
    {
        Assert.NotNull(error);
        Assert.Equal(DomainErrorCatalog.Types.RelatedEntitiesNotFound, error.Type);
        Assert.NotNull(error.Details);

        var details = error.Details.ToArray();
        Assert.Equal(ids.Length, details.Length);
        for (var i = 0; i < ids.Length; i++)
            EntityNotFound(details[i], ids[i]);
    }

    private static void AssertIdProperty(Error error, Id id)
    {
        Assert.NotNull(error.Properties);

        var name = DomainErrorCatalog.Properties.GetId(id);
        Assert.True(error.Properties.TryGet(name, out var property));
        Assert.Equal(id.ToString(), property.Value);
    }
}
