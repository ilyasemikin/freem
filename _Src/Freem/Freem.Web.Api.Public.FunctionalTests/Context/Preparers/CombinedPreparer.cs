namespace Freem.Web.Api.Public.FunctionalTests.Context.Preparers;

public class CombinedPreparer
{
    public ActivitiesPreparer Activities { get; }
    public TagsPreparer Tags { get; }
    public RecordsPreparer Records { get; }
    
    public UserPreparer Users { get; }

    public CombinedPreparer(TestContext context)
    {
        Activities = new ActivitiesPreparer(context);
        Tags = new TagsPreparer(context);
        Records = new RecordsPreparer(context);
        
        Users = new UserPreparer(context);
    }
}