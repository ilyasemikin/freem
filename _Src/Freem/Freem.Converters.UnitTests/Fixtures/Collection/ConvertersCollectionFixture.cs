using Freem.Converters.Collections;
using Freem.Converters.Collections.Builders;
using Freem.Converters.UnitTests.Mocks.Converters;
using Freem.Converters.UnitTests.Mocks.Entities.Abstractions;

namespace Freem.Converters.UnitTests.Fixtures.Collection;

public sealed class ConvertersCollectionFixture
{
    public ConvertersCollection<IInputEntity, IOutputEntity> Create()
    {
        return new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>()
            .Add(new FirstConverter())
            .Add(new SecondConverter())
            .Build();
    }

    public ConvertersCollection<IInputEntity, IOutputEntity> CreatePossible()
    {
        return new ConvertersCollectionBuilder<IInputEntity, IOutputEntity>()
            .Add(new FirstPossibleConverter())
            .Add(new SecondPossibleConverter())
            .Build();
    }
}