namespace Freem.Entities.UseCases.Contracts.Records.Update;

public enum UpdateRecordErrorCode
{
    RecordNotFound,
    RelatedActivitiesNotFound,
    RelatedTagsNotFound,
    RelatedUnknownNotFound,
    NothingToUpdate
}