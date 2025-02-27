using Freem.Entities.Records.Models;
using Freem.Entities.Relations.Collections;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Records.Create;

public sealed class CreateRecordRequest
{
    private readonly RelatedTagsCollection _tags = RelatedTagsCollection.Empty;
    
    public DateTimePeriod Period { get; }

    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public RelatedActivitiesCollection Activities { get; }

    public RelatedTagsCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _tags = value;
        }
    } 
    
    public CreateRecordRequest(DateTimePeriod period, RelatedActivitiesCollection activities)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Period = period;
        Activities = activities;
    }
}