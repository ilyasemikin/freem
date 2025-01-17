namespace Freem.Entities.UseCases.DTO.Records.Update;

public enum UpdateRecordErrorCode
{
    RecordNotFound,
    RelatedActivitiesNotFound,
    RelatedTagsNotFound,
    RelatedUnknownNotFound,
    NothingToUpdate
}