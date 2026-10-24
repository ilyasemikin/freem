using Freem.Domain.Storage.PostgreSQL.Database.Extensions;

namespace Freem.Domain.Storage.PostgreSQL.UnitTests.Tests.Database.Extensions;

public class DatabaseContextExtensionsTests
{
    [Theory]
    [InlineData("freem_test", true)]
    [InlineData("_test", true)]
    [InlineData("freem", false)]
    [InlineData("freem_test_prod", false)]
    [InlineData("Freem_TEST", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsTestDatabaseName_ShouldMatchSuffix(string? name, bool expected)
    {
        var actual = DatabaseContextExtensions.IsTestDatabaseName(name);

        Assert.Equal(expected, actual);
    }
}
