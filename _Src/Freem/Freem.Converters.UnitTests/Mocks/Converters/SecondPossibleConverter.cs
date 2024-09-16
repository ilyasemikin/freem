using System.Diagnostics.CodeAnalysis;
using Freem.Converters.Abstractions;
using Freem.Converters.UnitTests.Mocks.Entities;

namespace Freem.Converters.UnitTests.Mocks.Converters;

public sealed class SecondPossibleConverter : IPossibleConverter<SecondInputEntity, SecondOutputEntity>
{
    public bool TryConvert(SecondInputEntity input, [NotNullWhen(true)] out SecondOutputEntity? output)
    {
        output = null;
        if (input.Data != "Success")
            return false;

        output = new SecondOutputEntity("Success");
        return true;
    }
}