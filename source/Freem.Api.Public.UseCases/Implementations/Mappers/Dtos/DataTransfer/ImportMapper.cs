using Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos;
using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.Entities.DataTransfer.Import;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos.DataTransfer;

internal static class ImportMapper
{
    public static ImportDto Map(this ImportDocument document)
    {
        return new ImportDto(
            document.Id,
            document.Format,
            document.FileName,
            document.TimeZoneOffset,
            document.Status,
            document.RowsInFile,
            document.RowsParsed);
    }

    public static ImportTagDto Map(this ImportDocumentTag tag)
    {
        return new ImportTagDto(tag.Tag.Id, tag.Tag.Name)
        {
            MappedTagId = tag.MappedTagId
        };
    }

    public static ImportActivityDto Map(this ImportDocumentActivity activity)
    {
        var importActivity = activity.Activity;
        return new ImportActivityDto(importActivity.Id, importActivity.Name, importActivity.Status, importActivity.TagIds)
        {
            Color = importActivity.Color,
            MappedActivityId = activity.MappedActivityId
        };
    }

    public static ImportWarningDto Map(this ImportParseWarning warning)
    {
        return new ImportWarningDto(warning.LineNumber, warning.Message.ToString());
    }

    public static ImportRowDto Map(this ImportRow row)
    {
        var record = row.Record;
        var periods = record.Periods
            .Select(p => new ImportRowPeriodDto(p.Period, p.TimeZoneOffset) { Description = p.Description })
            .ToArray();
        return new ImportRowDto(row.Id, record.Id, record.ActivityIds, record.TagIds, periods)
        {
            Name = record.Name,
            Description = record.Description
        };
    }
}
