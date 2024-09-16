using Freem.EFCore.Extensions;
using Freem.Entities.Identifiers;
using Freem.Entities.Identifiers.Multiple;
using Freem.Entities.Storage.Abstractions.Exceptions;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events.Base;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Repositories.Base;
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
        var dbTagActual = await Database.Tags.FindAsync(tag.Id.Value);
        var dbEvent = await Database.Events.FindEventAsync<TagEventEntity>(e => e.TagId == tag.Id.Value);
        
        Assert.Equal(dbTag, dbTagActual);
        Assert.NotNull(dbEvent);
        Assert.Equal(EventAction.Created, dbEvent.Action);
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
        var dbTagActual = await Database.Tags.FirstOrDefaultAsync(e => e.Id == tag.Id.Value);
        var dbEvent = await Database.Events.FindEventAsync<TagEventEntity>(e => e.TagId == tag.Id.Value);
        
        Assert.NotEqual(dbTag, dbTagActual);
        Assert.Equal(dbUpdatedTag, dbTagActual);
        Assert.NotNull(dbEvent);
        Assert.Equal(EventAction.Updated, dbEvent.Action);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenEntityNotExists()
    {
        // Arrange
        var dbTag = EntitiesFactory.CreateTag();

        // Act
        var tag = dbTag.MapToDomainEntity();

        var exception = await Xunit.Record.ExceptionAsync(() => Repository.UpdateAsync(tag));
        
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
        
        await Repository.RemoveAsync(tag.Id);
        
        // Assert
        var dbTagActual = await Database.Tags.FindAsync(tag.Id.Value);
        var dbEvent = await Database.Events.FindEventAsync<TagEventEntity>(e => e.TagId == tag.Id.Value);

        Assert.Null(dbTagActual);
        Assert.NotNull(dbEvent);
        Assert.Equal(EventAction.Removed, dbEvent.Action);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenEntityIsNotExists()
    {
        // Arrange
        var idValue = Guid.NewGuid().ToString();
        var id = new TagIdentifier(idValue);
        
        // Act
        var exception = await Xunit.Record.ExceptionAsync(() => Repository.RemoveAsync(id));
        
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
        
        var tagId = new TagIdentifier(dbTag.Id);
        
        // Act
        var result = await Repository.FindByIdAsync(tagId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.Founded);
        Assert.NotNull(result.Entity);

        var tag = result.Entity;
        
        Assert.Equal(dbTag.Id, tag.Id.Value);
    }

    [Fact]
    public async Task FindByIdAsync_ShouldFailure_WhenTagDoesNotExist()
    {
        // Arrange
        var tagIdValue = Guid.NewGuid().ToString();
        var tagId = new TagIdentifier(tagIdValue);
        
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
        
        var tagId = new TagIdentifier(dbTag.Id);
        var userId = new UserIdentifier(dbUser.Id);
        var ids = new TagAndUserIdentifiers(tagId, userId);
        
        // Act
        var result = await Repository.FindAsync(ids);
        
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
        
        var tagIdValue = tagIdExists ? dbTag.Id : Guid.NewGuid().ToString();
        var userIdValue = userIdExists ? dbUser.Id : Guid.NewGuid().ToString();
        
        var tagId = new TagIdentifier(tagIdValue);
        var userId = new UserIdentifier(userIdValue);
        
        var ids = new TagAndUserIdentifiers(tagId, userId);
        
        // Act
        var result = await Repository.FindAsync(ids);
        
        // Assert
        Assert.NotNull(result);
        Assert.False(result.Founded);
        Assert.Null(result.Entity);
    }
}