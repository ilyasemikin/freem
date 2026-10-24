using Freem.Api.Public.Contracts.V1.Records.Running.Dtos;
using Freem.Api.Public.Contracts.V1.Records.Dtos;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Records.Running;
using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.RunningRecords;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class RecordMapper
{
    public static RecordDto Map(this Record record)
    {
        var periods = record.Periods
            .Select(p => new RecordPeriodDto(p.Id, p.Period, p.TimeZoneOffset) { Description = p.Description })
            .ToArray();
        return new RecordDto(record.Id, periods, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }

    public static RunningRecordDto Map(this RunningRecord record)
    {
        return new RunningRecordDto(record.Id, record.StartAt, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }
}
