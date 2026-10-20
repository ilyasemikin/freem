using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.RunningRecords;
using Freem.Api.Public.Contracts.DTO.Records;
using Freem.Api.Public.Contracts.DTO.Records.Running;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class RecordMapper
{
    public static RecordResponse Map(this Record record)
    {
        return new RecordResponse(record.Id, record.Period, record.Activities, record.Tags)
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
