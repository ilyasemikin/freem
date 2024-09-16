using Freem.Converters.Collections;
using Freem.Converters.Collections.Builders;
using Freem.Converters.UnitTests.Mocks.Converters;
using Freem.Converters.UnitTests.Mocks.Entities.Abstractions;

namespace Freem.Converters.UnitTests.Tests.Collections.Builders;

public sealed class ConvertersCollectionBuilderTests
{
    [Fact]
    public void Constructor_ShouldEmpty()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        Assert.Equal(0, builder.Count);
    }
    
    [Fact]
    public void TryAdd_ShouldSuccess_WhenPassTwoDifferentConverters()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();

        var success1 = builder.TryAdd(new FirstConverter());
        var success2 = builder.TryAdd(new SecondConverter());
        
        Assert.Equal(2, builder.Count);
        Assert.True(success1);
        Assert.True(success2);
    }

    [Fact]
    public void TryAdd_ShouldSuccess_WhenPassConverterAndPossibleConverter()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        var success1 = builder.TryAdd(new FirstConverter());
        var success2 = builder.TryAdd(new SecondPossibleConverter());
                
        Assert.Equal(2, builder.Count);
        Assert.True(success1);
        Assert.True(success2);
    }

    [Fact]
    public void TryAdd_ShouldFailure_WhenPassSameConverterTwice()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        var success1 = builder.TryAdd(new FirstConverter());
        var success2 = builder.TryAdd(new FirstConverter());
        
        Assert.Equal(1, builder.Count);
        Assert.True(success1);
        Assert.False(success2);
    }

    [Fact]
    public void TryAdd_ShouldSuccess_WhenPassTwoConvertersWithSameInputType()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        var success1 = builder.TryAdd(new FirstConverter());
        var success2 = builder.TryAdd(new FirstPossibleConverter());
        
        Assert.Equal(1, builder.Count);
        Assert.True(success1);
        Assert.False(success2);
    }

    [Fact]
    public void Add_ShouldSuccess_WhenPassTwoDifferentConverters()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        builder.Add(new FirstConverter());
        builder.Add(new SecondConverter());
        
        Assert.Equal(2, builder.Count);
    }
    
    [Fact]
    public void Add_ShouldSuccess_WhenPassConverterAndPossibleConverter()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        builder.Add(new FirstConverter());
        builder.Add(new SecondPossibleConverter());
                
        Assert.Equal(2, builder.Count);
    }

    [Fact]
    public void Add_ShouldThrowException_WhenPassSameConverterTwice()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        builder.Add(new FirstConverter());
        
        var exception = Record.Exception(() => builder.Add(new FirstConverter()));
        
        Assert.Equal(1, builder.Count);
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }
    
    [Fact]
    public void Add_ShouldThrowException_WhenPassTwoConvertersWithSameInputType()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        
        builder.Add(new FirstConverter());
        var exception = Record.Exception(() => builder.Add(new FirstPossibleConverter()));
        
        Assert.Equal(1, builder.Count);
        Assert.NotNull(exception);
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void Build_ShouldBuildCorrectCollection()
    {
        var builder = new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>();
        builder.Add(new FirstConverter());
        builder.Add(new SecondConverter());
        
        var collection = builder.Build();

        Assert.IsType<ConvertersCollection<IInputEntity, IOutputEntity>>(collection);
        Assert.Equal(builder.Count, collection.Count);
    }
}