using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Domain.Entities.Import;
using Freem.Domain.Import;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;

internal static class ImportMapper
{
    public static ImportResponse Map(this ImportDocument document)
    {
        var tags = document.Tags
            .Select(tag => tag.Map())
            .ToArray();
        var activities = document.Activities
            .Select(activity => activity.Map())
            .ToArray();
        return new ImportResponse(
            document.Id,
            document.Format,
            document.FileName,
            document.TimeZoneOffset,
            document.Status,
            document.RowsInFile,
            document.RowsParsed,
            tags,
            activities);
    }

    public static ImportTagResponse Map(this ImportDocumentTag tag)
    {
        var resolution = tag.Resolution;
        var response = new ImportResolutionResponse<Domain.Tags.TagId>(resolution?.IsCreateNew ?? false, resolution?.ExistingId);
        return new ImportTagResponse(tag.Source.Id, tag.Source.Name)
        {
            Resolution = resolution is null ? null : response
        };
    }

    public static ImportActivityResponse Map(this ImportDocumentActivity activity)
    {
        var source = activity.Source;
        var resolution = activity.Resolution;
        var response = new ImportResolutionResponse<Domain.Activities.ActivityId>(resolution?.IsCreateNew ?? false, resolution?.ExistingId);
        return new ImportActivityResponse(source.Id, source.Name, source.Status, source.TagIds)
        {
            Color = source.Color,
            Resolution = resolution is null ? null : response
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
