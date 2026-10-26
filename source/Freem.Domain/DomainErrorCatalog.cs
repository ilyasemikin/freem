using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Records;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Utilities.Errors;
using Utilities.Errors.Extensions;

namespace Freem.Domain;

public static class DomainErrorCatalog
{
    private const string Scope = "freem";

    public static class Types
    {
        public static readonly ErrorType EntityNotFound = ErrorType.Create(Scope, "entity_not_found");
        public static readonly ErrorType RelatedEntitiesNotFound = ErrorType.Create(Scope, "related_entities_not_found");
        public static readonly ErrorType EntityAlreadyExists = ErrorType.Create(Scope, "entity_already_exists");
        public static readonly ErrorType NothingToDo = ErrorType.Create(Scope, "nothing_to_do");
        public static readonly ErrorType PeriodIsInvalid = ErrorType.Create(Scope, "period_is_invalid");
        public static readonly ErrorType InvalidRequest = ErrorType.Create(Scope, "invalid_request");
        public static readonly ErrorType FailedDependency = ErrorType.Create(Scope, "failed_dependency");
        public static readonly ErrorType EventDeserializationFailed = ErrorType.Create(Scope, "event:deserialization_failed");

        public static readonly ErrorType AuthenticationRejected = ErrorType.Create(Scope, "auth:rejected");
        public static readonly ErrorType UserNotRegistered = ErrorType.Create(Scope, "user:not_registered");
        public static readonly ErrorType ActivityInvalidStatus = ErrorType.Create(Scope, "activity:invalid_status");
        public static readonly ErrorType ActivityNameAlreadyExists = ErrorType.Create(Scope, "activity:name_already_exists");
        public static readonly ErrorType TagNameAlreadyExists = ErrorType.Create(Scope, "tag:name_already_exists");
        public static readonly ErrorType ImportInvalidStatus = ErrorType.Create(Scope, "import:invalid_status");
        public static readonly ErrorType ImportUnmappedEntities = ErrorType.Create(Scope, "import:unmapped_entities");
        public static readonly ErrorType ImportTooManyRows = ErrorType.Create(Scope, "import:too_many_rows");
        public static readonly ErrorType FormatNotFound = ErrorType.Create(Scope, "format:not_found");

        public static readonly ErrorType InternalServerError = ErrorType.Create(Scope, "internal_server_error");
    }

    public static class Properties
    {
        public const string ActivityId = $"{Scope}:activity_id";
        public const string EventId = $"{Scope}:event_id";
        public const string EventType = $"{Scope}:event_type";
        public const string ImportActivityId = $"{Scope}:import_activity_id";
        public const string ImportDocumentId = $"{Scope}:import_document_id";
        public const string ImportRowId = $"{Scope}:import_row_id";
        public const string ImportTagId = $"{Scope}:import_tag_id";
        public const string RecordId = $"{Scope}:record_id";
        public const string RecordPeriodId = $"{Scope}:record_period_id";
        public const string RunningRecordId = $"{Scope}:running_record_id";
        public const string TagId = $"{Scope}:tag_id";
        public const string UserId = $"{Scope}:user_id";

        public const string ActivityStatus = $"{Scope}:activity_status";
        public const string TagName = $"{Scope}:tag_name";
        public const string FormatKey = $"{Scope}:format_key";
        public const string RowsLimit = $"{Scope}:rows_limit";
    }

    public static class Factories
    {
        public static Error CreateActivityNotFound(ActivityId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Activity with id = [{id}] not found")
                .WithProperty(Properties.ActivityId, id);
        }

        public static Error CreateEventNotFound(EventId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Event with id = [{id}] not found")
                .WithProperty(Properties.EventId, id);
        }

        public static Error CreateImportActivityNotFound(ImportActivityId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Import activity with id = [{id}] not found")
                .WithProperty(Properties.ImportActivityId, id);
        }

        public static Error CreateImportDocumentNotFound(ImportDocumentId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Import document with id = [{id}] not found")
                .WithProperty(Properties.ImportDocumentId, id);
        }

        public static Error CreateImportRowNotFound(ImportRowId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Import row with id = [{id}] not found")
                .WithProperty(Properties.ImportRowId, id);
        }

        public static Error CreateImportTagNotFound(ImportTagId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Import tag with id = [{id}] not found")
                .WithProperty(Properties.ImportTagId, id);
        }

        public static Error CreateRecordNotFound(RecordId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Record with id = [{id}] not found")
                .WithProperty(Properties.RecordId, id);
        }

        public static Error CreateRecordPeriodNotFound(RecordPeriodId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Record period with id = [{id}] not found")
                .WithProperty(Properties.RecordPeriodId, id);
        }

        public static Error CreateRunningRecordNotFound(RunningRecordId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Running record with id = [{id}] not found")
                .WithProperty(Properties.RunningRecordId, id);
        }

        public static Error CreateTagNotFound(TagId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Tag with id = [{id}] not found")
                .WithProperty(Properties.TagId, id);
        }

        public static Error CreateUserNotFound(UserId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"User with id = [{id}] not found")
                .WithProperty(Properties.UserId, id);
        }

        public static Error CreateRelatedEntitiesNotFound(IEnumerable<Error> details)
        {
            return new Error.Builder()
                .WithType(Types.RelatedEntitiesNotFound)
                .WithMessage("Related entities not found")
                .WithDetails(details);
        }

        public static Error CreateRunningRecordAlreadyExists(RunningRecordId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityAlreadyExists)
                .WithMessage($"Running record with id = [{id}] already exists")
                .WithProperty(Properties.RunningRecordId, id);
        }

        public static Error CreateUserAlreadyExists(UserId id)
        {
            return new Error.Builder()
                .WithType(Types.EntityAlreadyExists)
                .WithMessage($"User with id = [{id}] already exists")
                .WithProperty(Properties.UserId, id);
        }

        public static Error CreateEventDeserializationFailed(string eventType, string reason)
        {
            return new Error.Builder()
                .WithType(Types.EventDeserializationFailed)
                .WithMessage($"Failed to deserialize event \"{eventType}\": {reason}")
                .WithProperty(Properties.EventType, eventType);
        }

        public static Error CreateNothingToDo(ErrorMessage? message = null)
        {
            message ??= "Nothing to do";
            return new Error.Builder()
                .WithType(Types.NothingToDo)
                .WithMessage(message);
        }

        public static Error CreatePeriodIsInvalid(ErrorMessage message)
        {
            return new Error.Builder()
                .WithType(Types.PeriodIsInvalid)
                .WithMessage(message);
        }

        public static Error CreateInvalidRequest(ErrorMessage message)
        {
            return new Error.Builder()
                .WithType(Types.InvalidRequest)
                .WithMessage(message);
        }

        public static Error CreateFailedDependency(ErrorMessage message)
        {
            return new Error.Builder()
                .WithType(Types.FailedDependency)
                .WithMessage(message);
        }

        public static Error CreateAuthenticationRejected(ErrorMessage message)
        {
            return new Error.Builder()
                .WithType(Types.AuthenticationRejected)
                .WithMessage(message);
        }

        public static Error CreateUserNotRegistered(UserId id)
        {
            return new Error.Builder()
                .WithType(Types.UserNotRegistered)
                .WithMessage($"User with id = [{id}] is not registered")
                .WithProperty(Properties.UserId, id);
        }

        public static Error CreateActivityInvalidStatus(ActivityId id, ActivityStatus status)
        {
            return new Error.Builder()
                .WithType(Types.ActivityInvalidStatus)
                .WithMessage($"Activity with id = [{id}] has invalid status \"{status}\"")
                .WithProperty(Properties.ActivityId, id)
                .WithProperty(Properties.ActivityStatus, status);
        }

        public static Error CreateTagNotFound(TagName name)
        {
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Tag with name = [{name}] not found")
                .WithProperty(Properties.TagName, name);
        }

        public static Error CreateActivityNameAlreadyExists(ActivityId id)
        {
            return new Error.Builder()
                .WithType(Types.ActivityNameAlreadyExists)
                .WithMessage($"Name of activity with id = [{id}] already exists")
                .WithProperty(Properties.ActivityId, id);
        }

        public static Error CreateTagNameAlreadyExists(TagId id)
        {
            return new Error.Builder()
                .WithType(Types.TagNameAlreadyExists)
                .WithMessage($"Name of tag with id = [{id}] already exists")
                .WithProperty(Properties.TagId, id);
        }

        public static Error CreateImportInvalidStatus(ImportDocumentId id, object current, object requested)
        {
            return new Error.Builder()
                .WithType(Types.ImportInvalidStatus)
                .WithMessage($"Import document with id = [{id}] cannot change status from \"{current}\" to \"{requested}\"")
                .WithProperty(Properties.ImportDocumentId, id);
        }

        public static Error CreateImportUnmappedEntities(
            IEnumerable<ImportActivityId> activityIds,
            IEnumerable<ImportTagId> tagIds)
        {
            var activityDetails = activityIds.Select(CreateImportUnmappedActivity);
            var tagDetails = tagIds.Select(CreateImportUnmappedTag);
            var details = activityDetails.Concat(tagDetails);
            return new Error.Builder()
                .WithType(Types.ImportUnmappedEntities)
                .WithMessage("Import activities and tags used by selected rows are not mapped")
                .WithDetails(details);
        }

        public static Error CreateImportTooManyRows(int limit)
        {
            return new Error.Builder()
                .WithType(Types.ImportTooManyRows)
                .WithMessage($"Too many rows selected, limit is {limit}")
                .WithProperty(Properties.RowsLimit, limit);
        }

        public static Error CreateFormatNotFound(FormatKey key)
        {
            return new Error.Builder()
                .WithType(Types.FormatNotFound)
                .WithMessage($"Format with key = [{key}] not found")
                .WithProperty(Properties.FormatKey, key);
        }

        public static Error CreateInternalServerError(Error? error = null)
        {
            return new Error.Builder()
                .WithType(Types.InternalServerError)
                .WithMessage("Internal server error")
                .WithDetailIfNotNull(error);
        }

        public static Error CreateInternalServerError(ErrorMessage message)
        {
            return new Error.Builder()
                .WithType(Types.InternalServerError)
                .WithMessage(message);
        }

        private static Error CreateImportUnmappedActivity(ImportActivityId id)
        {
            return new Error.Builder()
                .WithType(Types.ImportUnmappedEntities)
                .WithMessage($"Import activity with id = [{id}] is not mapped")
                .WithProperty(Properties.ImportActivityId, id);
        }

        private static Error CreateImportUnmappedTag(ImportTagId id)
        {
            return new Error.Builder()
                .WithType(Types.ImportUnmappedEntities)
                .WithMessage($"Import tag with id = [{id}] is not mapped")
                .WithProperty(Properties.ImportTagId, id);
        }
    }
}
