using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Entities.Import;
using Freem.Domain.Import;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class ImportMapper
{
    public static ImportResponse Map(this ImportDocument document)
    {
        return new ImportResponse(
            document.Id,
            document.Format,
            document.FileName,
            document.TimeZoneOffset,
            document.Status,
            document.RowsInFile,
            document.RowsParsed);
    }

    public static ImportTagResponse Map(this ImportDocumentTag tag)
    {
        return new ImportTagResponse(tag.Tag.Id, tag.Tag.Name)
        {
            MappedTagId = tag.MappedTagId
        };
    }

    public static ImportActivityResponse Map(this ImportDocumentActivity activity)
    {
        var importActivity = activity.Activity;
        return new ImportActivityResponse(importActivity.Id, importActivity.Name, importActivity.Status, importActivity.TagIds)
        {
            Color = importActivity.Color,
            MappedActivityId = activity.MappedActivityId
        };
    }

    public static ImportWarningResponse Map(this ImportParseWarning warning)
    {
        return new ImportWarningResponse(warning.LineNumber, warning.Message.ToString());
    }

    public static ImportRowResponse Map(this ImportRow row)
    {
        var record = row.Record;
        var periods = record.Periods
            .Select(p => new ImportRowPeriodResponse(p.Period, p.TimeZoneOffset) { Description = p.Description })
            .ToArray();
        return new ImportRowResponse(row.Id, record.Id, record.ActivityIds, record.TagIds, periods)
        {
            Name = record.Name,
            Description = record.Description
        };
    }
}
