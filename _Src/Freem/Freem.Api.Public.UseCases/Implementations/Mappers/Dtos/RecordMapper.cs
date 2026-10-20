using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.RunningRecords;
using Freem.Api.Public.Contracts.DTO.Records;
using Freem.Api.Public.Contracts.DTO.Records.Running;
using Freem.Time.Models;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class RecordMapper
{
    public static RecordResponse Map(this Record record)
    {
        // TODO: Публичный API переедет на список периодов в этапе 2 (plans/record-periods)
        var startAt = record.Periods.First().Period.StartAt;
        var endAt = record.Periods.Last().Period.EndAt;
        var period = new DateTimePeriod(startAt, endAt);
        return new RecordResponse(record.Id, period, record.Activities, record.Tags)
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
