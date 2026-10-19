using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Records.Create;

public sealed class CreateRecordRequest
{
    private readonly RelatedTagIdsCollection _tags = RelatedTagIdsCollection.Empty;
    
    public DateTimePeriod Period { get; }

    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public RelatedActivityIdsCollection Activities { get; }

    public RelatedTagIdsCollection Tags
    {
        get => _tags;
        init
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _tags = value;
        }
    } 
    
    public CreateRecordRequest(DateTimePeriod period, RelatedActivityIdsCollection activities)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Period = period;
        Activities = activities;
    }
}