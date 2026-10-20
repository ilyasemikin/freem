using Freem.Domain;
using Freem.Domain.Storage.Models.Ids;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Tags;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.DataFactories;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class TagsRepositoryTests : BaseRepositoryTests<ITagRepository>
{
    public TagsRepositoryTests(
        ITestOutputHelper output) 
        : base(output)
    {
    }

    [Fact]
    public async Task CreateAsync_ShouldSuccess_WhenPassValid()
    {
        // Arrange
        var user = EntitiesFactory.User;
        
        await Database.Users.AddAsync(user);
        await Database.SaveChangesAsync();

        var dbTag = EntitiesFactory.CreateTag();

        // Act
        var tag = dbTag.MapToDomainEntity();
        
        var writeResult = await Repository.CreateAsync(tag);
        
        // Assert
        Assert.True(writeResult.Success);

        var tagId = tag.Id.ToString();
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tagId);

        Assert.Equal(dbTag, dbTagActual);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSuccess_WhenPassValid()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();

        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        Database.Entry(dbTag).State = EntityState.Detached;

        var dbUpdatedTag = dbTag.Copy();
        dbUpdatedTag.Name = "Updated";
        dbUpdatedTag.UpdatedAt = DateTimeOffset.UtcNow;

        // Act
        var tag = dbUpdatedTag.MapToDomainEntity();
        
        var writeResult = await Repository.UpdateAsync(tag);

        // Assert
        Assert.True(writeResult.Success);

        var tagId = tag.Id.ToString();
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tagId);
        
        Assert.NotEqual(dbTag, dbTagActual);
        Assert.Equal(dbUpdatedTag, dbTagActual);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateCreatedEntity_WhenEntityIsNotUpdatedActually()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();

        var tag = dbTag.MapToDomainEntity();
        
        // Act
        var writeResult = await Repository.UpdateAsync(tag);
        
        // Assert
        Assert.True(writeResult.Success);

        var tagId = tag.Id.ToString();
        var dbActualTag = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tagId);
        
        Assert.NotNull(dbActualTag);
        Assert.Null(dbActualTag.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnError_WhenEntityNotExists()
    {
        // Arrange
        var dbTag = EntitiesFactory.CreateTag();

        // Act
        var tag = dbTag.MapToDomainEntity();

        var result = await Repository.UpdateAsync(tag);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.EntityNotFound(result.Error, tag.Id);
    }

    [Fact]
    public async Task RemoveAsync_ShouldSuccess_WhenTagExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();

        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        Database.Entry(dbTag).State = EntityState.Detached;

        // Act
        var tag = dbTag.MapToDomainEntity();
        
        var writeResult = await Repository.DeleteAsync(tag.Id);
        
        // Assert
        Assert.True(writeResult.Success);

        var tagId = tag.Id.ToString();
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tagId);

        Assert.Null(dbTagActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnError_WhenEntityIsNotExists()
    {
        // Arrange
        var id = TagId.Generate();
        
        // Act
        var result = await Repository.DeleteAsync(id);
        
        // Assert
        Assert.False(result.Success);
        ErrorAssert.EntityNotFound(result.Error, id);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldSuccess_WhenTagExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        
        var tagId = TagId.Parse(dbTag.Id, null);
        
        // Act
        var result = await Repository.FindByIdAsync(tagId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);

        var tag = result.Value;
        
        var actualId = tag.Id.ToString();
        Assert.Equal(dbTag.Id, actualId);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenTagDoesNotExist()
    {
        // Arrange
        var tagId = TagId.Generate();
        
        // Act
        var result = await Repository.FindByIdAsync(tagId);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }

    [Fact]
    public async Task FindAsync_ShouldSuccess_WhenTagExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        
        var tagId = TagId.Parse(dbTag.Id, null);
        var userId = UserId.Parse(dbUser.Id, null);
        var ids = new TagAndUserIds(tagId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Value);

        var tag = result.Value;
        
        Assert.NotNull(tag);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public async Task FindAsync_ShouldFailure_WhenPartOfIdsNotExists(bool tagIdExists, bool userIdExists)
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();

        var tagId = tagIdExists ? TagId.Parse(dbTag.Id, null) : TagId.Generate();
        var userId = userIdExists ? UserId.Parse(dbUser.Id, null) : GenerateUserId();
        
        var ids = new TagAndUserIds(tagId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(DomainErrorCatalog.Types.EntityNotFound, result.Error.Type);
    }
}