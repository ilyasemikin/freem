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
        return result switch
        {
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToUniversalTime(),
            DateTime dateTime => dateTime.ToUniversalTime(),
            _ => result
        };
    }
}
