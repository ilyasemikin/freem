using System.Diagnostics.CodeAnalysis;
using Freem.Converters.Abstractions;
using Freem.Converters.UnitTests.Mocks.Entities;

namespace Freem.Converters.UnitTests.Mocks.Converters;

public sealed class FirstPossibleConverter : IPossibleConverter<FirstInputEntity, FirstOutputEntity>
{
    public bool TryConvert(FirstInputEntity input, [NotNullWhen(true)] out FirstOutputEntity? output)
    {
        output = null;
        if (input.Data != "Success")
            return false;

        output = new FirstOutputEntity("Success");
        return true;
    }
}