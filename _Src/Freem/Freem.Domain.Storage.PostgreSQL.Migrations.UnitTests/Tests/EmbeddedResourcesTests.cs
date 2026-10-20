using System.Reflection;
using Freem.Domain.Storage.PostgreSQL.Migrations.Instances;

namespace Freem.Domain.Storage.PostgreSQL.Migrations.UnitTests.Tests;

public class EmbeddedResourcesTests
{
    [Theory]
    [InlineData(RecreateDbTriggersMigration.EntitiesTriggersScriptFileName)]
    [InlineData(RecreateDbTriggersMigration.ExceptionFunctionsScriptFileName)]
    [InlineData(EventsDbConstraintMigration.EventsTriggersScriptFileName)]
    public void EmbeddedResources_ShouldContain(string name)
    {
        var assembly = Assembly.Load("Freem.Domain.Storage.PostgreSQL.Migrations");
        using var stream = assembly.GetManifestResourceStream(name);
        
        Assert.NotNull(stream);
    }
}