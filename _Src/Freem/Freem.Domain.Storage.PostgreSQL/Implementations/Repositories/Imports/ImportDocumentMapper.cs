using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Imports;

internal static class ImportDocumentMapper
{
    public static ImportDocumentEntity MapToDatabaseEntity(this ImportDocument entity)
    {
        return new ImportDocumentEntity
        {
            Id = entity.Id.ToString(),
            UserId = entity.UserId.ToString(),
            Format = entity.Format.ToString(),
            FileName = entity.FileName.ToString(),
            TimeZoneOffsetTicks = entity.TimeZoneOffset.Ticks,
            Status = entity.Status.ToString(),
            RowsInFile = entity.RowsInFile,
            RowsParsed = entity.RowsParsed
        };
    }

    public static ImportDocumentTagEntity MapToDatabaseEntity(this ImportDocumentTag entity, string documentId)
    {
        return new ImportDocumentTagEntity
        {
            DocumentId = documentId,
            ImportTagId = entity.Tag.Id.ToString(),
            Name = entity.Tag.Name.ToString(),
            MappedTagId = entity.MappedTagId?.ToString()
        };
    }

    public static ImportDocumentActivityEntity MapToDatabaseEntity(
        this ImportDocumentActivity entity,
        string documentId)
    {
        var activity = entity.Activity;
        var activityId = activity.Id.ToString();
        var tags = activity.TagIds
            .Select(t => new ImportDocumentActivityTagEntity
            {
                DocumentId = documentId,
                ImportActivityId = activityId,
                ImportTagId = t.ToString()
            })
            .ToList();
        return new ImportDocumentActivityEntity
        {
            DocumentId = documentId,
            ImportActivityId = activityId,
            Name = activity.Name.ToString(),
            Color = activity.Color?.ToString(),
            Status = activity.Status.ToString(),
            MappedActivityId = entity.MappedActivityId?.ToString(),
            Tags = tags
        };
    }

    public static ImportDocument MapToDomainEntity(this ImportDocumentEntity entity)
    {
        var id = ImportDocumentId.Parse(entity.Id, null);
        var userId = UserId.Parse(entity.UserId, null);
        var format = FormatKey.Parse(entity.Format, null);
        var fileName = ImportFileName.Parse(entity.FileName, null);
        var offsetValue = new TimeSpan(entity.TimeZoneOffsetTicks);
        var timeZoneOffset = new TimeZoneOffset(offsetValue);
        var status = ImportDocumentStatus.Parse(entity.Status, null);

        return new ImportDocument(
            id,
            userId,
            format,
            fileName,
            timeZoneOffset,
            status,
            entity.RowsInFile,
            entity.RowsParsed);
    }

    public static ImportDocumentTag MapToDomainEntity(this ImportDocumentTagEntity entity)
    {
        var id = ImportTagId.Parse(entity.ImportTagId, null);
        var name = TagName.Parse(entity.Name, null);
        var tag = new ImportTag(id, name);

        var mappedTagId = entity.MappedTagId is null
            ? null
            : TagId.Parse(entity.MappedTagId, null);
        return new ImportDocumentTag(tag, mappedTagId);
    }

    public static ImportDocumentActivity MapToDomainEntity(this ImportDocumentActivityEntity entity)
    {
        var id = ImportActivityId.Parse(entity.ImportActivityId, null);
        var name = ActivityName.Parse(entity.Name, null);
        var color = entity.Color is null
            ? null
            : ActivityColor.Parse(entity.Color, null);
        var status = ActivityStatus.Parse(entity.Status, null);
        var tagIds = (entity.Tags ?? [])
            .OrderBy(t => t.ImportTagId, StringComparer.Ordinal)
            .Select(t => ImportTagId.Parse(t.ImportTagId, null))
            .ToList();
        var activity = new ImportActivity(id, name, color, status, tagIds);

        var mappedActivityId = entity.MappedActivityId is null
            ? null
            : ActivityId.Parse(entity.MappedActivityId, null);
        return new ImportDocumentActivity(activity, mappedActivityId);
    }
}
