using AutoFixture;
using AutoFixture.Kernel;

namespace Freem.AutoFixture.SpecimenBuilders;

public class UtcRandomDateTimeSequenceGenerator : ISpecimenBuilder
{
    private readonly ISpecimenBuilder _innerRandomDateTimeSequenceGenerator;

    public UtcRandomDateTimeSequenceGenerator()
    {
        _innerRandomDateTimeSequenceGenerator = new RandomDateTimeSequenceGenerator();
    }

    public object Create(object request, ISpecimenContext context)
    {
        var result = _innerRandomDateTimeSequenceGenerator.Create(request, context);
        if (result is NoSpecimen)
            return result;
        else if (result is DateTimeOffset dateTimeOffset)
            return dateTimeOffset.ToUniversalTime();
        else if (result is DateTime dateTime)
            return dateTime.ToUniversalTime();

        return result;
    }
}
