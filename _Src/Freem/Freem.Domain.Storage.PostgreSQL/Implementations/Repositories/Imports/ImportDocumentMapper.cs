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
        var id = entity.Id.ToString();
        var tags = entity.Tags
            .Select(t => t.MapToDatabaseEntity(id))
            .ToList();
        var activities = entity.Activities
            .Select(a => a.MapToDatabaseEntity(id))
            .ToList();
        return new ImportDocumentEntity
        {
            Id = id,
            UserId = entity.UserId.ToString(),
            Format = entity.Format.ToString(),
            FileName = entity.FileName.ToString(),
            TimeZoneOffsetTicks = entity.TimeZoneOffset.Ticks,
            Status = entity.Status.ToString(),
            Tags = tags,
            Activities = activities
        };
    }

    public static ImportDocumentTagEntity MapToDatabaseEntity(this ImportDocumentTag entity, string documentId)
    {
        var resolution = entity.Resolution;
        return new ImportDocumentTagEntity
        {
            DocumentId = documentId,
            SourceId = entity.Source.Id.ToString(),
            Name = entity.Source.Name.ToString(),
            CreateNew = resolution is not null && resolution.IsCreateNew,
            ResolvedTagId = resolution?.ExistingId?.ToString()
        };
    }

    public static ImportDocumentActivityEntity MapToDatabaseEntity(
        this ImportDocumentActivity entity,
        string documentId)
    {
        var source = entity.Source;
        var sourceId = source.Id.ToString();
        var resolution = entity.Resolution;
        var tags = source.TagIds
            .Select(t => new ImportDocumentActivityTagEntity
            {
                DocumentId = documentId,
                ActivitySourceId = sourceId,
                TagSourceId = t.ToString()
            })
            .ToList();
        return new ImportDocumentActivityEntity
        {
            DocumentId = documentId,
            SourceId = sourceId,
            Name = source.Name.ToString(),
            Color = source.Color?.ToString(),
            Status = source.Status.ToString(),
            CreateNew = resolution is not null && resolution.IsCreateNew,
            ResolvedActivityId = resolution?.ExistingId?.ToString(),
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

        var tags = (entity.Tags ?? [])
            .OrderBy(t => t.SourceId, StringComparer.Ordinal)
            .Select(t => t.MapToDomainEntity())
            .ToList();
        var activities = (entity.Activities ?? [])
            .OrderBy(a => a.SourceId, StringComparer.Ordinal)
            .Select(a => a.MapToDomainEntity())
            .ToList();

        return new ImportDocument(id, userId, format, fileName, timeZoneOffset, status, tags, activities);
    }

    public static ImportDocumentTag MapToDomainEntity(this ImportDocumentTagEntity entity)
    {
        var id = ImportTagId.Parse(entity.SourceId, null);
        var name = TagName.Parse(entity.Name, null);
        var source = new ImportTag(id, name);

        ImportTagResolution? resolution = null;
        if (entity.CreateNew)
        {
            resolution = ImportTagResolution.CreateNew;
        }
        else if (entity.ResolvedTagId is not null)
        {
            var tagId = TagId.Parse(entity.ResolvedTagId, null);
            resolution = ImportTagResolution.Existing(tagId);
        }

        return new ImportDocumentTag(source, resolution);
    }

    public static ImportDocumentActivity MapToDomainEntity(this ImportDocumentActivityEntity entity)
    {
        var id = ImportActivityId.Parse(entity.SourceId, null);
        var name = ActivityName.Parse(entity.Name, null);
        var color = entity.Color is null
            ? null
            : ActivityColor.Parse(entity.Color, null);
        var status = ActivityStatus.Parse(entity.Status, null);
        var tagIds = (entity.Tags ?? [])
            .OrderBy(t => t.TagSourceId, StringComparer.Ordinal)
            .Select(t => ImportTagId.Parse(t.TagSourceId, null))
            .ToList();
        var source = new ImportActivity(id, name, color, status, tagIds);

        ImportActivityResolution? resolution = null;
        if (entity.CreateNew)
        {
            resolution = ImportActivityResolution.CreateNew;
        }
        else if (entity.ResolvedActivityId is not null)
        {
            var activityId = ActivityId.Parse(entity.ResolvedActivityId, null);
            resolution = ImportActivityResolution.Existing(activityId);
        }

        return new ImportDocumentActivity(source, resolution);
    }
}
