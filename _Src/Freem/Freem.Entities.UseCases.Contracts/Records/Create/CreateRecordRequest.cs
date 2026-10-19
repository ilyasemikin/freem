using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Records.Create;

public sealed class CreateRecordRequest
{
    private readonly RelatedTagsIdentifiersCollection _tags = RelatedTagsIdentifiersCollection.Empty;
    
    public DateTimePeriod Period { get; }

    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public RelatedActivitiesIdentifiersCollection Activities { get; }

    public RelatedTagsIdentifiersCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _tags = value;
        }
    } 
    
    public CreateRecordRequest(DateTimePeriod period, RelatedActivitiesIdentifiersCollection activities)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Period = period;
        Activities = activities;
    }
}