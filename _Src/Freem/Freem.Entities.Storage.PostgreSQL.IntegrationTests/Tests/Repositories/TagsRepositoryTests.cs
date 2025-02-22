using Freem.EFCore.Extensions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Models.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
using Freem.Entities.Tags.Identifiers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories;

public sealed class TagsRepositoryTests : BaseRepositoryTests<ITagsRepository>
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
        
        await Repository.CreateAsync(tag);
        
        // Assert
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tag.Id);

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
        Database.DetachEntry(dbTag);

        var dbUpdatedTag = dbTag.Clone();
        dbUpdatedTag.Name = "Updated";
        dbUpdatedTag.UpdatedAt = DateTimeOffset.UtcNow;

        // Act
        var tag = dbUpdatedTag.MapToDomainEntity();
        
        await Repository.UpdateAsync(tag);

        // Assert
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tag.Id);
        
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
        await Repository.UpdateAsync(tag);
        
        // Assert
        var dbActualTag = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tag.Id);
        
        Assert.NotNull(dbActualTag);
        Assert.Null(dbActualTag.UpdatedAt);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityNotExists()
    {
        // Arrange
        var dbTag = EntitiesFactory.CreateTag();

        // Act
        var tag = dbTag.MapToDomainEntity();

        var exception = await Record.ExceptionAsync(() => Repository.UpdateAsync(tag));
        
        // Assert
        Assert.NotNull(exception);
        
        var concreteException = Assert.IsType<NotFoundException>(exception);
        Assert.Equal(tag.Id, concreteException.Id);
    }

    [Fact]
    public async Task RemoveAsync_ShouldSuccess_WhenTagExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();

        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        Database.DetachEntry(dbTag);

        // Act
        var tag = dbTag.MapToDomainEntity();
        
        await Repository.DeleteAsync(tag.Id);
        
        // Assert
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tag.Id);

        Assert.Null(dbTagActual);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var id = IdentifiersGenerator.Generate<TagIdentifier>();
        
        // Act
        var exception = await Record.ExceptionAsync(() => Repository.DeleteAsync(id));
        
        // Assert
        var concreteException = Assert.IsType<NotFoundException>(exception);
        Assert.Equal(id, concreteException.Id);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldSuccess_WhenTagExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        
        var tagId = (TagIdentifier)dbTag.Id;
        
        // Act
        var result = await Repository.FindByIdAsync(tagId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);

        var tag = result.Entity;
        
        Assert.Equal(dbTag.Id, tag.Id);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenTagDoesNotExist()
    {
        // Arrange
        var tagId = IdentifiersGenerator.Generate<TagIdentifier>();
        
        // Act
        var result = await Repository.FindByIdAsync(tagId);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
    }

    [Fact]
    public async Task FindAsync_ShouldSuccess_WhenTagExists()
    {
        // Arrange
        var dbUser = EntitiesFactory.User;
        var dbTag = EntitiesFactory.CreateTag();
        
        await Database.AddRangeAsync(dbUser, dbTag);
        await Database.SaveChangesAsync();
        
        var tagId = (TagIdentifier)dbTag.Id;
        var userId = (UserIdentifier)dbUser.Id;
        var ids = new TagAndUserIdentifiers(tagId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);

        var tag = result.Entity;
        
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

        var tagId = tagIdExists ? new TagIdentifier(dbTag.Id) : IdentifiersGenerator.Generate<TagIdentifier>();
        var userId = userIdExists ? new UserIdentifier(dbUser.Id) : IdentifiersGenerator.Generate<UserIdentifier>();
        
        var ids = new TagAndUserIdentifiers(tagId, userId);
        
        // Act
        var result = await Repository.FindByMultipleIdAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
    }
}