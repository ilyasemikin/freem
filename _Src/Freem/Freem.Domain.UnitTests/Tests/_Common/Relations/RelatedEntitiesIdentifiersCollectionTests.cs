using Freem.Domain.Relations.Collections.Base;
using Freem.Domain.UnitTests.Fixtures.Identifiers;
using Freem.Domain.UnitTests.Mocs.Entities;
using Utilities.Collections;

// ReSharper disable once CheckNamespace
namespace Freem.Domain.UnitTests.Tests.Common.Relations;

public sealed class RelatedEntitiesIdentifiersCollectionTests : IClassFixture<EntitiesClassFixture>
{
    private readonly EntitiesClassFixture _fixture;

    public RelatedEntitiesIdentifiersCollectionTests(EntitiesClassFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData(
        10,
        RelatedEntitiesIdentifiersCollection<TestIdentifier>.DefaultMinCount,
        RelatedEntitiesIdentifiersCollection<TestIdentifier>.DefaultMaxCount)]
    [InlineData(0, 0, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(5, 5, 5)]
    public void Constructor_ShouldSuccess_WhenParametersValid(int identifiersCount, int minCount, int maxCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);

        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers, minCount, maxCount);

        Assert.Equal(identifiersCount, collection.Count);
        Assert.Equal(minCount, collection.MinCount);
        Assert.Equal(maxCount, collection.MaxCount);
        Assert.True(identifiers.UnorderedEquals(collection.Identifiers));
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(0, -1)]
    [InlineData(6, 5)]
    [InlineData(0, 0)]
    public void Constructor_ShouldThrowException_WhenCountsInvalid(int minCount, int maxCount)
    {
        var exception = Xunit.Record.Exception(
            () => new RelatedEntitiesIdentifiersCollection<TestIdentifier>([], minCount, maxCount));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Theory]
    [InlineData(2, 0, 1)]
    [InlineData(0, 1, 1)]
    [InlineData(0, 1, 10)]
    [InlineData(5, 6, 7)]
    [InlineData(8, 6, 7)]
    public void Constructor_ShouldThrowException_WhenIdentifiersCountInvalid(
        int identifiersCount, int minCount, int maxCount)
    {
        var identifiers = _fixture.CreateIdentifiers(identifiersCount);

        var exception = Xunit.Record.Exception(
            () => new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers, minCount, maxCount));

        Assert.NotNull(exception);
    }

    [Fact]
    public void TryAdd_ShouldSuccess_WhenCountValid()
    {
        var identifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>();

        var success = collection.TryAdd(identifier);

        Assert.True(success);
        Assert.Equal(1, collection.Count);
        Assert.Contains(identifier, collection.Identifiers);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenCountInvalid()
    {
        var identifiers = _fixture.CreateIdentifiers(1);
        var newIdentifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers, maxCount: 1);

        var success = collection.TryAdd(newIdentifier);

        Assert.False(success);
        Assert.Equal(1, collection.Count);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenAddedTwice()
    {
        var identifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>();

        collection.TryAdd(identifier);

        var success = collection.TryAdd(identifier);

        Assert.False(success);
    }

    [Fact]
    public void TryRemove_ShouldSuccess_WhenCountValid()
    {
        var identifiers = _fixture.CreateIdentifiers(1);
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers);

        var identifier = collection.Identifiers.First();

        var success = collection.TryRemove(identifier);

        Assert.True(success);
        Assert.Equal(0, collection.Count);
        Assert.Empty(collection.Identifiers);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenRemoveUnknown()
    {
        var identifier = _fixture.CreateIdentifier();
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>();

        var success = collection.TryRemove(identifier);

        Assert.False(success);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenCountInvalid()
    {
        var identifiers = _fixture.CreateIdentifiers(1);
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers, minCount: 1);

        var identifier = collection.Identifiers.First();

        var success = collection.TryRemove(identifier);

        Assert.False(success);
    }

    [Fact]
    public void Contains_ShouldSuccess_WhenCheckAnyOfIdentifiers()
    {
        var identifiers = _fixture.CreateIdentifiers(5);
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers);

        foreach (var id in identifiers)
        {
            var success = collection.Contains(id);

            Assert.True(success);
        }
    }

    [Fact]
    public void Contains_ShouldFailure_WhenCheckUnknownIdentifier()
    {
        var identifiers = _fixture.CreateIdentifiers(5);
        var unknownIdentifiers = _fixture.CreateIdentifiers(5);
        var collection = new RelatedEntitiesIdentifiersCollection<TestIdentifier>(identifiers);

        foreach (var id in unknownIdentifiers)
        {
            var success = collection.Contains(id);

            Assert.False(success);
        }
    }
}
