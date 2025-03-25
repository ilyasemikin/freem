namespace Freem.Entities.UseCases.Contracts.RunningRecords.Update;

public enum UpdateRunningRecordErrorCode
{
    RunningRecordNotFound,
    NothingToUpdate,
    RelatedActivitiesNotFound,
    RelatedTagsNotFound,
    RelatedUnknownNotFound
}