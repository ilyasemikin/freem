using Freem.Converters.UnitTests.Fixtures.Collection;
using Freem.Converters.UnitTests.Mocks.Entities;
using Freem.Converters.UnitTests.Mocks.Entities.Abstractions;

namespace Freem.Converters.UnitTests.Tests.Collections;

public sealed class ConvertersCollectionTests : IClassFixture<ConvertersCollectionFixture>
{
    private readonly ConvertersCollectionFixture _fixture;

    public ConvertersCollectionTests(ConvertersCollectionFixture fixture)
    {
        _fixture = fixture;
    }

    public static TheoryData<IInputEntity, IOutputEntity> TryConvertKnownTypesCases =>
        new()
        {
            { new FirstInputEntity("First"), new FirstOutputEntity("First") },
            { new SecondInputEntity("Second"), new SecondOutputEntity("Second") }
        };

    [Theory]
    [MemberData(nameof(TryConvertKnownTypesCases))]
    public void TryConvert_ShouldSuccess_WhenPassKnownType(IInputEntity entity, IOutputEntity expected)
    {
        var collection = _fixture.Create();
        
        var success = collection.TryConvert(entity, out var actual);
        
        Assert.True(success);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryConvert_ShouldFail_WhenPassUnknownType()
    {
        var collection = _fixture.Create();
        var entity = new ThirdInputEntity("Third");

        var success = collection.TryConvert(entity, out var actual);
        
        Assert.False(success);
        Assert.Null(actual);
    }
    
    public static TheoryData<IInputEntity, IOutputEntity> TryConvertPossibleKnownObjectCases =>
        new()
        {
            { new FirstInputEntity("Success"), new FirstOutputEntity("Success") },
            { new SecondInputEntity("Success"), new SecondOutputEntity("Success") }
        };
    
    [Theory]
    [MemberData(nameof(TryConvertPossibleKnownObjectCases))]
    public void TryConvert_ShouldSuccess_WhenPassKnownConvertableObject_WhenConvertersPossible(
        IInputEntity entity, IOutputEntity expected)
    {
        var collection = _fixture.CreatePossible();
        
        var success = collection.TryConvert(entity, out var actual);
        
        Assert.True(success);
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }
    
    public static TheoryData<IInputEntity> TryConvertPossibleUnconvertableObjectCases =>
        new()
        {
            { new FirstInputEntity("Unsuccess") },
            { new SecondInputEntity("Unsuccess") }
        };
    
    [Theory]
    [MemberData(nameof(TryConvertPossibleUnconvertableObjectCases))]
    public void TryConvert_ShouldFail_WhenPassUnconvertableObjectAndConvertersPossible(IInputEntity entity)
    {
        var collection = _fixture.CreatePossible();
        
        var success = collection.TryConvert(entity, out var actual);
        
        Assert.False(success);
        Assert.Null(actual);
    }
}