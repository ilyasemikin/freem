using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Time;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Records.Create;

public sealed class CreateRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.Create;

    private readonly RelatedTagIdsCollection _tags = RelatedTagIdsCollection.Empty;
    
    public IReadOnlyList<NewRecordPeriod> Periods { get; }

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
        : this([new NewRecordPeriod(period)], activities)
    {
    }

    public CreateRecordRequest(IReadOnlyList<NewRecordPeriod> periods, RelatedActivityIdsCollection activities)
    {
        ArgumentNullException.ThrowIfNull(periods);
        ArgumentNullException.ThrowIfNull(activities);
        
        Periods = periods;
        Activities = activities;
    }
}