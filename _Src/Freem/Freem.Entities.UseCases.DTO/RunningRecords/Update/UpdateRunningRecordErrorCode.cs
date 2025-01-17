namespace Freem.Entities.UseCases.DTO.RunningRecords.Update;

public enum UpdateRunningRecordErrorCode
{
    RunningRecordNotFound,
    NothingToUpdate,
    RelatedActivitiesNotFound,
    RelatedTagsNotFound,
    RelatedUnknownNotFound
}