using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Models;
using Npgsql;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.Database.Errors;

public sealed class DatabaseForeignKeyConstraintErrorTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenConstraintIsNull()
    {
        var column = new DatabaseColumn("table", "column");
        var columnWithValue = new DatabaseColumnWithValue(column, "value");

        var exception = Record.Exception(() => new DatabaseForeignKeyConstraintError(null!, columnWithValue));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("constraint", ((ArgumentNullException)exception).ParamName);
    }
    
    [Fact]
    public void Constructor_ShouldThrowException_WhenColumnIsNull()
    {
        var constraint = new DatabaseForeignKeyConstraintError.ConstraintInfo("table", "name");

        var exception = Record.Exception(() => new DatabaseForeignKeyConstraintError(constraint, null!));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("column", ((ArgumentNullException)exception).ParamName);
    }
    
    [Fact]
    public void TryParse_ShouldSuccess_WhenPassValid()
    {
        const string input = "insert or update on table \"activities_tags\" violates foreign key constraint \"activities_tags_tags_fk\"";
        const string message = "DETAIL: Key (tag_id)=(Id199bdd05-45a9-40ef-9a28-2d563b16b0df) is not present in table \"tags\".";

        var exception = new PostgresException(input, "Error", "Sample", "Sample", detail: message);
        
        var constraint = new DatabaseForeignKeyConstraintError.ConstraintInfo("activities_tags", "activities_tags_tags_fk");
        var column = new DatabaseColumn("tags", "tag_id");
        var columnWithValue = new DatabaseColumnWithValue(column, "Id199bdd05-45a9-40ef-9a28-2d563b16b0df");

        var expected = new DatabaseForeignKeyConstraintError(constraint, columnWithValue);
        
        var success = DatabaseForeignKeyConstraintError.TryParse(exception, out var actual);
        
        Assert.True(success);
        Assert.Equal(expected, actual);
    }
}