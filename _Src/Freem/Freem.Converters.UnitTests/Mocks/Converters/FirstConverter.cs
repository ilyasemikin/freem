using Freem.Converters.Abstractions;
using Freem.Converters.UnitTests.Mocks.Entities;

namespace Freem.Converters.UnitTests.Mocks.Converters;

public sealed class FirstConverter : IConverter<FirstInputEntity, FirstOutputEntity>
{
    public FirstOutputEntity Convert(FirstInputEntity input)
    {
        return new FirstOutputEntity(input.Data);
    }
}