using System.Reflection;
using Freem.Entities.Storage.PostgreSQL.Migrations.Instances;
using Freem.Reflection;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.UnitTests.Tests;

public class EmbeddedResourcesTests
{
    [Theory]
    [InlineData(RecreateDbTriggersMigration.EntitiesTriggersScriptFileName)]
    [InlineData(RecreateDbTriggersMigration.ExceptionFunctionsScriptFileName)]
    public void EmbeddedResources_ShouldContain(string name)
    {
        var assembly = Assembly.Load("Freem.Entities.Storage.PostgreSQL.Migrations");
        var exception = Record.Exception(() => ResourceLoader.Load(assembly, name));
        
        Assert.Null(exception);
    }
}