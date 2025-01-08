namespace Freem.Entities.UseCases.RunningRecords.Update.Models;

public enum UpdateRunningRecordErrorCode
{
    RunningRecordNotFound,
    NothingToUpdate,
    RelatedActivitiesNotFound,
    RelatedTagsNotFound,
    RelatedUnknownNotFound
}