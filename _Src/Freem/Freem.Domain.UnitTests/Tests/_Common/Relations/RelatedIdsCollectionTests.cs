using Freem.Domain.Relations.Collections.Base;
using Freem.Domain.UnitTests.Fixtures.Ids;
using Freem.Domain.UnitTests.Mocs.Entities;
using Utilities.Collections;

// ReSharper disable once CheckNamespace
namespace Freem.Domain.UnitTests.Tests.Common.Relations;

public sealed class RelatedIdsCollectionTests : IClassFixture<EntitiesClassFixture>
{
    private readonly EntitiesClassFixture _fixture;

    public RelatedIdsCollectionTests(EntitiesClassFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [InlineData(
        10,
        RelatedIdsCollection<TestId>.DefaultMinCount,
        RelatedIdsCollection<TestId>.DefaultMaxCount)]
    [InlineData(0, 0, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(5, 5, 5)]
    public void Constructor_ShouldSuccess_WhenParametersValid(int idsCount, int minCount, int maxCount)
    {
        var ids = _fixture.CreateIds(idsCount);

        var collection = new RelatedIdsCollection<TestId>(ids, minCount, maxCount);

        Assert.Equal(idsCount, collection.Count);
        Assert.Equal(minCount, collection.MinCount);
        Assert.Equal(maxCount, collection.MaxCount);
        Assert.True(ids.UnorderedEquals(collection.Ids));
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(0, -1)]
    [InlineData(6, 5)]
    [InlineData(0, 0)]
    public void Constructor_ShouldThrowException_WhenCountsInvalid(int minCount, int maxCount)
    {
        var exception = Xunit.Record.Exception(
            () => new RelatedIdsCollection<TestId>([], minCount, maxCount));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Theory]
    [InlineData(2, 0, 1)]
    [InlineData(0, 1, 1)]
    [InlineData(0, 1, 10)]
    [InlineData(5, 6, 7)]
    [InlineData(8, 6, 7)]
    public void Constructor_ShouldThrowException_WhenIdsCountInvalid(
        int idsCount, int minCount, int maxCount)
    {
        var ids = _fixture.CreateIds(idsCount);

        var exception = Xunit.Record.Exception(
            () => new RelatedIdsCollection<TestId>(ids, minCount, maxCount));

        Assert.NotNull(exception);
    }

    [Fact]
    public void TryAdd_ShouldSuccess_WhenCountValid()
    {
        var id = _fixture.CreateId();
        var collection = new RelatedIdsCollection<TestId>();

        var success = collection.TryAdd(id);

        Assert.True(success);
        Assert.Equal(1, collection.Count);
        Assert.Contains(id, collection.Ids);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenCountInvalid()
    {
        var ids = _fixture.CreateIds(1);
        var newId = _fixture.CreateId();
        var collection = new RelatedIdsCollection<TestId>(ids, maxCount: 1);

        var success = collection.TryAdd(newId);

        Assert.False(success);
        Assert.Equal(1, collection.Count);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenAddedTwice()
    {
        var id = _fixture.CreateId();
        var collection = new RelatedIdsCollection<TestId>();

        collection.TryAdd(id);

        var success = collection.TryAdd(id);

        Assert.False(success);
    }

    [Fact]
    public void TryRemove_ShouldSuccess_WhenCountValid()
    {
        var ids = _fixture.CreateIds(1);
        var collection = new RelatedIdsCollection<TestId>(ids);

        var id = collection.Ids.First();

        var success = collection.TryRemove(id);

        Assert.True(success);
        Assert.Equal(0, collection.Count);
        Assert.Empty(collection.Ids);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenRemoveUnknown()
    {
        var id = _fixture.CreateId();
        var collection = new RelatedIdsCollection<TestId>();

        var success = collection.TryRemove(id);

        Assert.False(success);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenCountInvalid()
    {
        var ids = _fixture.CreateIds(1);
        var collection = new RelatedIdsCollection<TestId>(ids, minCount: 1);

        var id = collection.Ids.First();

        var success = collection.TryRemove(id);

        Assert.False(success);
    }

    [Fact]
    public void Contains_ShouldSuccess_WhenCheckAnyOfIds()
    {
        var ids = _fixture.CreateIds(5);
        var collection = new RelatedIdsCollection<TestId>(ids);

        foreach (var id in ids)
        {
            var success = collection.Contains(id);

            Assert.True(success);
        }
    }

    [Fact]
    public void Contains_ShouldFailure_WhenCheckUnknownId()
    {
        var ids = _fixture.CreateIds(5);
        var unknownIds = _fixture.CreateIds(5);
        var collection = new RelatedIdsCollection<TestId>(ids);

        foreach (var id in unknownIds)
        {
            var success = collection.Contains(id);

            Assert.False(success);
        }
    }
}
