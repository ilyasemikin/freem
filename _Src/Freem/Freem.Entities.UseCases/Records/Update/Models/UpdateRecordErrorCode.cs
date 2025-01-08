namespace Freem.Entities.UseCases.Records.Update.Models;

public enum UpdateRecordErrorCode
{
    RecordNotFound,
    RelatedActivitiesNotFound,
    RelatedTagsNotFound,
    RelatedUnknownNotFound,
    NothingToUpdate
}