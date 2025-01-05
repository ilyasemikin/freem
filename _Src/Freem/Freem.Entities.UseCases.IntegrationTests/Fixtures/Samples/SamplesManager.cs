using Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples;

public sealed class SamplesManager
{
    public UsersSampleManager Users { get; }
    
    public ActivitiesSampleManager Activities { get; }
    public RecordsSampleManager Records { get; }

    public SamplesManager(ServicesContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Users = new UsersSampleManager(context);
        Activities = new ActivitiesSampleManager(context);
        Records = new RecordsSampleManager(context);
    }
}