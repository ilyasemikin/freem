using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.RunningRecords;
using Freem.Api.Public.Contracts.DTO.Records;
using Freem.Api.Public.Contracts.DTO.Records.Running;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class RecordMapper
{
    public static RecordResponse Map(this Record record)
    {
        var periods = record.Periods
            .Select(p => new RecordPeriodResponse(p.Id, p.Period) { Description = p.Description })
            .ToArray();
        return new RecordResponse(record.Id, periods, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }

    public static RunningRecordResponse Map(this RunningRecord record)
    {
        return new RunningRecordResponse(record.Id, record.StartAt, record.Activities, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }
}
