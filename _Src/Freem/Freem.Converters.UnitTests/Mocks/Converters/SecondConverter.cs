using Freem.Converters.Abstractions;
using Freem.Converters.UnitTests.Mocks.Entities;

namespace Freem.Converters.UnitTests.Mocks.Converters;

public sealed class SecondConverter : IConverter<SecondInputEntity, SecondOutputEntity>
{
    public SecondOutputEntity Convert(SecondInputEntity input)
    {
        return new SecondOutputEntity(input.Data);
    }
}