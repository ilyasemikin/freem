using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Entities.Storage.PostgreSQL.Database.Models;

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
        const string input =
            $"23503: insert or update on table \"categories_tags\" violates foreign key constraint \"categories_tags_tags_fk\"\n\n" +
            $"DETAIL: Key (tag_id)=(tag_id) is not present in table \"tags\".";

        var constraint = new DatabaseForeignKeyConstraintError.ConstraintInfo("categories_tags", "categories_tags_tags_fk");
        var column = new DatabaseColumn("tags", "tag_id");
        var columnWithValue = new DatabaseColumnWithValue(column, "tag_id");

        var expected = new DatabaseForeignKeyConstraintError(constraint, columnWithValue);
        
        var success = DatabaseForeignKeyConstraintError.TryParse(input, out var actual);
        
        Assert.True(success);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void TryParse_ShouldSuccess_WhenPassValidWithWinNewLine()
    {
        const string input =
            $"23503: insert or update on table \"categories_tags\" violates foreign key constraint \"categories_tags_tags_fk\"\r\n\r\n" +
            $"DETAIL: Key (tag_id)=(tag_id) is not present in table \"tags\".";

        var constraint = new DatabaseForeignKeyConstraintError.ConstraintInfo("categories_tags", "categories_tags_tags_fk");
        var column = new DatabaseColumn("tags", "tag_id");
        var columnWithValue = new DatabaseColumnWithValue(column, "tag_id");

        var expected = new DatabaseForeignKeyConstraintError(constraint, columnWithValue);
        
        var success = DatabaseForeignKeyConstraintError.TryParse(input, out var actual);
        
        Assert.True(success);
        Assert.Equal(expected, actual);
    }
}