using Freem.Collections.Extensions;
using Freem.Entities.Common.Relations.Collections.Base;
using Freem.Entities.UnitTests.Fixtures.Identifiers;
using Freem.Entities.UnitTests.Mocs.Entities;

namespace Freem.Entities.UnitTests.Tests.Relations;

public sealed class RelatedEntitiesCollectionTests : IClassFixture<EntitiesClassFixture>
{
    private readonly EntitiesClassFixture _fixture;

    public RelatedEntitiesCollectionTests(EntitiesClassFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Theory]
    [InlineData(
        0, 10, 
        RelatedEntitiesCollection<TestEntity, TestIdentifier>.DefaultMinCount, 
        RelatedEntitiesCollection<TestEntity, TestIdentifier>.DefaultMaxCount)]
    [InlineData(
        10, 0, 
        RelatedEntitiesCollection<TestEntity, TestIdentifier>.DefaultMinCount, 
        RelatedEntitiesCollection<TestEntity, TestIdentifier>.DefaultMaxCount)]
    [InlineData(
        5, 5, 
        RelatedEntitiesCollection<TestEntity, TestIdentifier>.DefaultMinCount, 
        RelatedEntitiesCollection<TestEntity, TestIdentifier>.DefaultMaxCount)]
    [InlineData(5, 5, 10, 10)]
    [InlineData(1, 0, 0, 1)]
    [InlineData(0, 1, 0, 1)]
    [InlineData(1, 0, 0, 2)]
    [InlineData(0, 1, 0, 2)]
    public void Constructor_ShouldSuccess_WhenParametersValid(
        int identifiersCount, int entitiesCount,
        int minCount, int maxCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);
        var entities = _fixture.CreateEntities(entitiesCount);

        var expectedIdentifiers = identifiers.Concat(entities.Select(identifier => identifier.Id)).Distinct();
        var expectedEntities = entities;

        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(
            identifiers, entities, 
            minCount, maxCount);
        
        Assert.Equal(identifiersCount + entitiesCount, collection.Count);
        Assert.Equal(minCount, collection.MinCount);
        Assert.Equal(maxCount, collection.MaxCount);
        Assert.True(expectedIdentifiers.UnorderedEquals(collection.Identifiers));
        Assert.True(expectedEntities.UnorderedEquals(collection.Entities));
    }
    
    [Theory]
    [InlineData(-1, 10)]
    [InlineData(0, -1)]
    [InlineData(6, 5)]
    [InlineData(0, 0)]
    public void Constructor_ShouldThrowException_WhenCountsInvalid(int minCount, int maxCount)
    {
        var exception = Xunit.Record.Exception(() => new RelatedEntitiesCollection<TestEntity, TestIdentifier>([], [], minCount, maxCount));
        
        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }
    
    [Theory]
    [InlineData(2, 0, 0, 1)]
    [InlineData(0, 2, 0, 1)]
    [InlineData(1, 1, 0, 1)]
    [InlineData(0, 0, 1, 1)]
    [InlineData(0, 0, 1, 10)]
    [InlineData(5, 0, 6, 7)]
    [InlineData(0, 5, 6, 7)]
    [InlineData(8, 0, 6, 7)]
    [InlineData(0, 8, 6, 7)]
    public void Constructor_ShouldThrowException_WhenIdentifiersAndEntitiesCountInvalid(
        int identifiersCount, int entitiesCount,
        int minCount, int maxCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);
        var entities = _fixture.CreateEntities(entitiesCount);
        
        var exception = Xunit.Record.Exception(() => new RelatedEntitiesCollection<TestEntity, TestIdentifier>(
            identifiers, entities, minCount, maxCount));
        
        Assert.NotNull(exception);
    }

    [Fact]
    public void TryAddIdentifier_ShouldSuccess_WhenIdentifiersAndEntitiesCountValid()
    {
        var identifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>();

        var success = collection.TryAdd(identifier);
        
        Assert.True(success);
        Assert.Equal(1, collection.Count);
        Assert.NotEmpty(collection.Identifiers);
        Assert.Empty(collection.Entities);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void TryAddIdentifier_ShouldFailure_WhenIdentifiersAndEntitiesCountInvalid(
        int identifiersCount, int entitiesCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);
        var entities = _fixture.CreateEntities(entitiesCount);
        var newIdentifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities, maxCount: 1);
        
        var success = collection.TryAdd(newIdentifier);
        
        Assert.False(success);
        Assert.Equal(1, collection.Count);
    }

    [Fact]
    public void TryAddIdentifier_ShouldFailure_WhenAddedTwice()
    {
        var identifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>();
        
        collection.TryAdd(identifier);
        
        var success = collection.TryAdd(identifier);
        
        Assert.False(success);
    }

    [Fact]
    public void TryAddEntity_ShouldSuccess_WhenIdentifiersAndEntitiesCountValid()
    {
        var entity = _fixture.CreateEntity();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>();
        
        var success = collection.TryAdd(entity);

        Assert.True(success);
        Assert.Equal(1, collection.Count);
        Assert.NotEmpty(collection.Identifiers);
        Assert.NotEmpty(collection.Entities);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void TryAddEntity_ShouldFailure_WhenIdentifiersAndEntitiesCountInvalid(
        int identifiersCount, int entitiesCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);
        var entities = _fixture.CreateEntities(entitiesCount);
        var newEntity = _fixture.CreateEntity();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities, maxCount: 1);
        
        var success = collection.TryAdd(newEntity);
        
        Assert.False(success);
        Assert.Equal(1, collection.Count);
    }

    [Fact]
    public void TryAddEntity_ShouldFailure_WhenAddedTwice()
    {
        var entity = _fixture.CreateEntity();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>();
        
        collection.TryAdd(entity);
        
        var success = collection.TryAdd(entity);
        
        Assert.False(success);
    }

    [Fact]
    public void TryUpdate_ShouldSuccess_WhenUpdateIdentifierOnlyEntity()
    {
        var identifier = _fixture.CreateIdentifier();
        var entity = _fixture.CreateEntity();
        entity = new TestEntity(identifier, entity.Data);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers: [identifier]);

        var success = collection.TryUpdate(entity);
        
        collection.TryGet(identifier, out var actual);
        
        Assert.True(success);
        Assert.Equal(entity, actual);
    }

    [Fact]
    public void TryUpdate_ShouldSuccess_WhenUpdateEntity()
    {
        var entity = _fixture.CreateEntity();
        var identifier = entity.Id;
        entity = _fixture.CreateEntity();
        entity = new TestEntity(identifier, entity.Data);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(entities: [entity]);
        
        var success = collection.TryUpdate(entity);

        collection.TryGet(identifier, out var actual);
        
        Assert.True(success);
        Assert.Equal(entity, actual);
    }

    [Fact]
    public void TryUpdate_ShouldFailure_WhenUpdateUnknownEntity()
    {
        var entity = _fixture.CreateEntity();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>();

        var success = collection.TryUpdate(entity);
        
        Assert.False(success);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void TryRemove_ShouldSuccess_WhenIdentifiersAndEntitiesCountInvalid(int identifiersCount, int entitiesCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);
        var entities = _fixture.CreateEntities(entitiesCount);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities);

        var identifier = collection.Identifiers.First();
        
        var success = collection.TryRemove(identifier);
        
        Assert.True(success);
        Assert.Equal(0, collection.Count);
        Assert.Empty(collection.Identifiers);
        Assert.Empty(collection.Entities);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenRemoveUnknown()
    {
        var identifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>();
        
        var success = collection.TryRemove(identifier);
        
        Assert.False(success);
    }
    
    [Theory]
    [InlineData(1, 0)]
    [InlineData(0, 1)]
    public void TryRemove_ShouldFailure_WhenIdentifiersAndEntitiesCountInvalid(int identifiersCount, int entitiesCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);
        var entities = _fixture.CreateEntities(entitiesCount);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities, minCount: 1);
        
        var identifier = collection.Identifiers.First();
        
        var success = collection.TryRemove(identifier);
        
        Assert.False(success);
    }

    [Fact]
    public void TryGet_ShouldSuccess_WhenGetAnyOfIdentifiers()
    {
        var identifiers = _fixture.CreateIdentifiers(5);
        var entities = _fixture.CreateEntities(5);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities);

        var expectedEntities = entities.ToDictionary(e => e.Id);
        foreach (var id in entities.Select(e => e.Id))
        {
            var success = collection.TryGet(id, out var actual);
            
            var expected = expectedEntities[id];
            
            Assert.True(success);
            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void TryGet_ShouldFailure_WhenGetIdentifierOnlyEntity()
    {
        var identifiers = _fixture.CreateIdentifiers(5);
        var entities = _fixture.CreateEntities(5);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities);

        foreach (var id in identifiers)
        {
            var success = collection.TryGet(id, out var actual);
            
            Assert.False(success);
            Assert.Null(actual);
        }
    }

    [Fact]
    public void TryGet_ShouldFailure_WhenGetUnknownIdentifier()
    {
        var entities = _fixture.CreateEntities(5);
        var identifiers = _fixture.CreateIdentifiers(5);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(entities: entities);

        foreach (var id in identifiers)
        {
            var success = collection.TryGet(id, out var actual);
            
            Assert.False(success);
            Assert.Null(actual);
        }
    }

    [Fact]
    public void Contains_ShouldSuccess_WhenCheckAnyOfIdentifiers()
    {
        var identifiers = _fixture.CreateIdentifiers(5);
        var entities = _fixture.CreateEntities(5);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(identifiers, entities);

        foreach (var id in identifiers.Concat(entities.Select(e => e.Id)))
        {
            var success = collection.Contains(id);
            
            Assert.True(success);
        }
    }

    [Fact]
    public void Contains_ShouldFailure_WhenCheckUnknownIdentifier()
    {
        var identifiers = _fixture.CreateIdentifiers(5);
        var entities = _fixture.CreateEntities(5);
        var collection = new RelatedEntitiesCollection<TestEntity, TestIdentifier>(entities: entities);

        foreach (var id in identifiers)
        {
            var success = collection.Contains(id);
            
            Assert.False(success);
        }
    }
}