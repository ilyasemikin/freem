using Freem.Entities.Storage.PostgreSQL.Database.Errors.Implementations;

namespace Freem.Entities.Storage.PostgreSQL.UnitTests.Tests.Database.Errors;

public class DatabaseForeignKeyConstraintErrorTests
{
    [Fact]
    public void Constructor_ShouldThrowException_WhenTableNameInNull()
    {
        var exception = Record.Exception(() => new DatabaseForeignKeyConstraintError(null!, "ForeignKey"));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("tableName", ((ArgumentNullException)exception).ParamName);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenForeignKeyNameInNull()
    {
        var exception = Record.Exception(() => new DatabaseForeignKeyConstraintError("TableName", null!));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
        Assert.Equal("foreignKeyName", ((ArgumentNullException)exception).ParamName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_ShouldThrowException_WhenTableNameInWhiteSpace(string tableName)
    {
        var exception = Record.Exception((() => new DatabaseForeignKeyConstraintError(tableName, "ForeignKeyName")));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("tableName", ((ArgumentException)exception).ParamName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Constructor_ShouldThrowException_WhenForeignKeyNameInWhiteSpace(string foreignKeyName)
    {
        var exception = Record.Exception((() => new DatabaseForeignKeyConstraintError("TableName", foreignKeyName)));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("foreignKeyName", ((ArgumentException)exception).ParamName);
    }
    
    [Theory]
    [InlineData(
        "  insert or update on table \"categories_tags\" violates foreign key constraint \"categories_tags_tags_fk\"  ",
        "categories_tags", 
        "categories_tags_tags_fk")]
    [InlineData(
        "insert or update on table \"categories_tags\" violates foreign key constraint \"categories_tags_tags_fk\"",
        "categories_tags", 
        "categories_tags_tags_fk")]
    [InlineData(
        "insert or update on table \"records_tags\" violates foreign key constraint \"records_tags_tags_fk\"",
        "records_tags",
        "records_tags_tags_fk")]
    public void TryParse_ShouldSuccess_WhenPassValid(
        string input,
        string expectedTableName,
        string expectedForeignKeyName)
    {
        var expected = new DatabaseForeignKeyConstraintError(expectedTableName, expectedForeignKeyName);

        var success = DatabaseForeignKeyConstraintError.TryParse(input, out var error);

        Assert.True(success);
        Assert.Equal(expected, error);
    }
}