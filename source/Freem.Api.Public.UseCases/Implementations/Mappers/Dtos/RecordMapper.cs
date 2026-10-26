using Freem.Api.Public.Contracts.V1.Records.Running.Dtos;
using Freem.Api.Public.Contracts.V1.Records.Dtos;
using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.Contracts.V1.Records.Running;
using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Records.Sources;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class RecordMapper
{
    public static RecordDto Map(this Record record)
    {
        var periods = record.Periods
            .Select(p => new RecordPeriodDto(p.Id, p.Period, p.TimeZoneOffset) { Description = p.Description })
            .ToArray();
        var source = record.Source.Map();
        return new RecordDto(record.Id, periods, record.Activities, source, record.Tags)
        {
            Name = record.Name,
            Description = record.Description
        };
    }

    public static RecordSourceDto Map(this RecordSource source)
    {
        var importSource = source as ImportRecordSource;
        return new RecordSourceDto(source.Type)
        {
            ImportId = importSource?.DocumentId,
            ImportRowId = importSource?.RowId,
            Format = importSource?.Format,
            ImportRecordId = importSource?.ImportRecordId
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
