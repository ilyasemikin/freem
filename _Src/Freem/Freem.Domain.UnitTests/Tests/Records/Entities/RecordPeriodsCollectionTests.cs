using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.UnitTests.Tests.Records.Entities;

public sealed class RecordPeriodsCollectionTests
{
    private static readonly DateTimeOffset Base = new(2026, 10, 20, 10, 0, 0, TimeSpan.Zero);

    private static RecordPeriod CreatePeriod(int startMinutes, int endMinutes)
    {
        var id = RecordPeriodId.Generate();
        var startAt = Base.AddMinutes(startMinutes);
        var endAt = Base.AddMinutes(endMinutes);
        var period = new DateTimePeriod(startAt, endAt);
        return new RecordPeriod(id, period, TimeZoneOffset.Default);
    }

    [Fact]
    public void Constructor_ShouldSortByStartAt()
    {
        var later = CreatePeriod(60, 90);
        var earlier = CreatePeriod(0, 30);

        var collection = new RecordPeriodsCollection(later, earlier);

        Assert.Equal([earlier, later], collection);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenEmpty()
    {
        var exception = Xunit.Record.Exception(() => new RecordPeriodsCollection());

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPeriodsOverlap()
    {
        var first = CreatePeriod(0, 30);
        var second = CreatePeriod(29, 60);

        var exception = Xunit.Record.Exception(() => new RecordPeriodsCollection(first, second));

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Constructor_ShouldAllowTouchingPeriods()
    {
        var first = CreatePeriod(0, 30);
        var second = CreatePeriod(30, 60);

        var collection = new RecordPeriodsCollection(first, second);

        Assert.Equal(2, collection.Count);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenIdsDuplicate()
    {
        var first = CreatePeriod(0, 30);
        var duplicate = new RecordPeriod(first.Id, new DateTimePeriod(Base.AddMinutes(60), Base.AddMinutes(90)), TimeZoneOffset.Default);

        var exception = Xunit.Record.Exception(() => new RecordPeriodsCollection(first, duplicate));

        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void TryCreate_ShouldSuccess_AndSort_WhenPeriodsAreValid()
    {
        var later = CreatePeriod(60, 90);
        var earlier = CreatePeriod(0, 30);

        var success = RecordPeriodsCollection.TryCreate([later, earlier], out var collection);

        Assert.True(success);
        Assert.Equal([earlier, later], collection);
    }

    [Fact]
    public void TryCreate_ShouldFailure_WhenEmptyOrOverlapping()
    {
        Assert.False(RecordPeriodsCollection.TryCreate([], out var empty));
        Assert.Null(empty);

        Assert.False(RecordPeriodsCollection.TryCreate([CreatePeriod(0, 30), CreatePeriod(10, 40)], out var overlapping));
        Assert.Null(overlapping);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenOverlaps()
    {
        var collection = new RecordPeriodsCollection(CreatePeriod(0, 30));

        var success = collection.TryAdd(CreatePeriod(10, 40));

        Assert.False(success);
        Assert.Single(collection);
    }

    [Fact]
    public void TryAdd_ShouldKeepOrder_WhenSuccess()
    {
        var last = CreatePeriod(60, 90);
        var collection = new RecordPeriodsCollection(last);
        var first = CreatePeriod(0, 30);

        var success = collection.TryAdd(first);

        Assert.True(success);
        Assert.Equal([first, last], collection);
    }

    [Fact]
    public void TryReplace_ShouldFailure_WhenIdUnknown()
    {
        var collection = new RecordPeriodsCollection(CreatePeriod(0, 30));

        var success = collection.TryReplace(CreatePeriod(60, 90));

        Assert.False(success);
    }

    [Fact]
    public void TryReplace_ShouldFailure_WhenOverlapsAnother()
    {
        var first = CreatePeriod(0, 30);
        var second = CreatePeriod(60, 90);
        var collection = new RecordPeriodsCollection(first, second);
        var moved = new RecordPeriod(second.Id, new DateTimePeriod(Base.AddMinutes(20), Base.AddMinutes(90)), TimeZoneOffset.Default);

        var success = collection.TryReplace(moved);

        Assert.False(success);
        Assert.Equal([first, second], collection);
    }

    [Fact]
    public void TryReplace_ShouldSuccess_WhenPeriodOnlyShrinks()
    {
        var first = CreatePeriod(0, 30);
        var collection = new RecordPeriodsCollection(first);
        var shrunk = new RecordPeriod(first.Id, new DateTimePeriod(Base, Base.AddMinutes(10)), TimeZoneOffset.Default);

        var success = collection.TryReplace(shrunk);

        Assert.True(success);
        Assert.Equal([shrunk], collection);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenLastPeriod()
    {
        var only = CreatePeriod(0, 30);
        var collection = new RecordPeriodsCollection(only);

        var success = collection.TryRemove(only.Id);

        Assert.False(success);
        Assert.Single(collection);
    }

    [Fact]
    public void TryRemove_ShouldSuccess_WhenPeriodExistsAndNotLast()
    {
        var first = CreatePeriod(0, 30);
        var second = CreatePeriod(60, 90);
        var collection = new RecordPeriodsCollection(first, second);

        var success = collection.TryRemove(first.Id);

        Assert.True(success);
        Assert.Equal([second], collection);
    }

    [Fact]
    public void TryRemove_ShouldFailure_WhenIdUnknown()
    {
        var collection = new RecordPeriodsCollection(CreatePeriod(0, 30), CreatePeriod(60, 90));

        var success = collection.TryRemove(RecordPeriodId.Generate());

        Assert.False(success);
        Assert.Equal(2, collection.Count);
    }
}
