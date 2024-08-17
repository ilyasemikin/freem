using Freem.DateTimePeriods;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordMapper
{
    public static RecordEntity MapToDatabaseEntity(this Record entity)
    {
        return new RecordEntity
        {
            Id = entity.Id.Value,
            Name = entity.Name,
            Description = entity.Description,
            UserId = entity.UserId.Value,
        };
    }

    public static IEnumerable<RecordCategoryRelationEntity> MapToRecordCategoryRelations(this Record entity)
    {
        return entity.Categories.Identifiers.Select(id => MapToRecordCategoryRelation(entity.Id, id.Value));
    }

    public static IEnumerable<RecordTagRelationEntity> MapToRecordTagRelations(this Record entity)
    {
        return entity.Tags.Identifiers.Select(id => MapToRecordTagRelation(entity.Id, id.Value));
    }

    public static RecordCategoryRelationEntity MapToRecordCategoryRelation(RecordIdentifier recordId, string categoryIdString)
    {
        return new RecordCategoryRelationEntity
        {
            RecordId = recordId.Value,
            CategoryId = categoryIdString,
        };
    }

    public static RecordTagRelationEntity MapToRecordTagRelation(RecordIdentifier recordId, string tagIdString)
    {
        return new RecordTagRelationEntity
        {
            RecordId = recordId.Value,
            TagId = tagIdString
        };
    }

    public static Record MapToDomainEntity(this RecordEntity entity)
    {
        var id = new RecordIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);

        var categories = entity.Categories.MapToRelatedCategoriesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();

        var period = new DateTimePeriod(entity.StartAt, entity.EndAt);

        return new Record(id, userId, categories, tags, period);
    }
}
