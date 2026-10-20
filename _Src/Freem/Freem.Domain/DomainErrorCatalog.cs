using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Utilities.Errors;
using Utilities.Errors.Extensions;

namespace Freem.Domain;

public static class DomainErrorCatalog
{
    public const string Scope = "freem";

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
        public static readonly ErrorType TagNameAlreadyExists = ErrorType.Create(Scope, "tag:name_already_exists");

        public static readonly ErrorType InternalServerError = ErrorType.Create(Scope, "internal_server_error");
    }

    public static class Properties
    {
        public const string ActivityId = $"{Scope}:activity_id";
        public const string EventId = $"{Scope}:event_id";
        public const string EventType = $"{Scope}:event_type";
        public const string RecordId = $"{Scope}:record_id";
        public const string RunningRecordId = $"{Scope}:running_record_id";
        public const string TagId = $"{Scope}:tag_id";
        public const string UserId = $"{Scope}:user_id";

        public const string ActivityStatus = $"{Scope}:activity_status";
        public const string TagName = $"{Scope}:tag_name";

        public static string GetId(Id id)
        {
            return id switch
            {
                Activities.ActivityId => ActivityId,
                Events.EventId => EventId,
                Records.RecordId => RecordId,
                RunningRecords.RunningRecordId => RunningRecordId,
                Tags.TagId => TagId,
                Users.UserId => UserId,
                _ => throw new ArgumentOutOfRangeException(nameof(id), id.GetType(), string.Empty)
            };
        }
    }

    public static class Factories
    {
        private const string EventEntityName = "event";

        public static Error CreateEntityNotFound(Id id)
        {
            var name = GetEntityName(id);
            var property = Properties.GetId(id);
            return new Error.Builder()
                .WithType(Types.EntityNotFound)
                .WithMessage($"Entity \"{name}\" with id = [{id}] not found")
                .WithProperty(property, id);
        }

        public static Error CreateRelatedEntitiesNotFound(IEnumerable<Id> ids)
        {
            var details = ids.Select(CreateEntityNotFound);
            return new Error.Builder()
                .WithType(Types.RelatedEntitiesNotFound)
                .WithMessage("Related entities not found")
                .WithDetails(details);
        }

        public static Error CreateEntityAlreadyExists(Id id)
        {
            var name = GetEntityName(id);
            var property = Properties.GetId(id);
            return new Error.Builder()
                .WithType(Types.EntityAlreadyExists)
                .WithMessage($"Entity \"{name}\" with id = [{id}] already exists")
                .WithProperty(property, id);
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

        public static Error CreateTagNameAlreadyExists(TagId id)
        {
            return new Error.Builder()
                .WithType(Types.TagNameAlreadyExists)
                .WithMessage($"Name of tag with id = [{id}] already exists")
                .WithProperty(Properties.TagId, id);
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

        private static string GetEntityName(Id id)
        {
            return id is EventId
                ? EventEntityName
                : EntityIdNameProvider.Instance.Get(id);
        }
    }
}
