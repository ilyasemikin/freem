using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Records;
using EntityRecord = Freem.Domain.Entities.Records.Record;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.Users;
using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Domain.UnitTests.Tests.Events;

public sealed class AggregateEventsTests
{
    private readonly UserId _userId = UserId.Generate();
    private readonly ActivityId _activityId = ActivityId.Generate();
    private readonly TagId _tagId = TagId.Generate();

    [Fact]
    public void Tag_ShouldBuildCreatedEventWithSnapshot()
    {
        var tagId = TagId.Generate();
        var name = TagName.Parse("name", null);
        var tag = new Tag(tagId, _userId, name);

        var @event = tag.BuildCreatedEvent(EventId.Generate());

        Assert.Equal(EventTypes.Tags.Created, @event.Type);
        Assert.Equal(new EntityRef(EntityType.Tag, tagId.ToString()), @event.EntityRef);
        Assert.Equal(tagId, @event.TagId);
        Assert.Equal(name, @event.Name);
    }

    [Fact]
    public void Tag_ShouldBuildRemovedEventWithEntityRef()
    {
        var tagId = TagId.Generate();
        var tag = new Tag(tagId, _userId, TagName.Parse("name", null));

        var @event = tag.BuildRemovedEvent(EventId.Generate());

        Assert.Equal(EventTypes.Tags.Removed, @event.Type);
        Assert.Equal(new EntityRef(EntityType.Tag, tagId.ToString()), @event.EntityRef);
    }

    [Fact]
    public void Activity_ShouldBuildUpdatedEventWithSnapshot()
    {
        var name = ActivityName.Parse("name", null);
        var tags = new RelatedTagIdsCollection(_tagId);
        var activity = new Activity(_activityId, _userId, name, tags, ActivityStatus.Archived);

        var @event = activity.BuildUpdatedEvent(EventId.Generate());

        Assert.Equal(EventTypes.Activities.Updated, @event.Type);
        Assert.Equal(new EntityRef(EntityType.Activity, _activityId.ToString()), @event.EntityRef);
        Assert.Equal(name, @event.Name);
        Assert.Equal(ActivityStatus.Archived, @event.Status);
        Assert.Equal([_tagId], @event.TagIds);
    }

    [Fact]
    public void Activity_ShouldBuildArchivedAndUnarchivedEventsWithDifferentTypes()
    {
        var name = ActivityName.Parse("name", null);
        var tags = new RelatedTagIdsCollection();
        var activity = new Activity(_activityId, _userId, name, tags, ActivityStatus.Active);

        var archived = activity.BuildArchivedEvent(EventId.Generate());
        var unarchived = activity.BuildUnarchivedEvent(EventId.Generate());

        Assert.Equal(EventTypes.Activities.Archived, archived.Type);
        Assert.Equal(EventTypes.Activities.Unarchived, unarchived.Type);
    }

    [Fact]
    public void Record_ShouldBuildCreatedEventWithSnapshot()
    {
        var recordId = RecordId.Generate();
        var activities = new RelatedActivityIdsCollection(_activityId);
        var tags = new RelatedTagIdsCollection(_tagId);
        var period = new DateTimePeriod(DateTimeOffset.UtcNow.AddHours(-1), TimeSpan.FromHours(1));
        var recordPeriod = new RecordPeriod(RecordPeriodId.Generate(), period);
        var periods = new RecordPeriodsCollection(recordPeriod);
        var record = new EntityRecord(recordId, _userId, activities, tags, periods)
        {
            Name = RecordName.Parse("name", null)
        };

        var @event = record.BuildCreatedEvent(EventId.Generate());

        Assert.Equal(EventTypes.Records.Created, @event.Type);
        Assert.Equal(new EntityRef(EntityType.Record, recordId.ToString()), @event.EntityRef);
        Assert.Equal(record.Name, @event.Name);
        Assert.Null(@event.Description);
        var snapshot = Assert.Single(@event.Periods);
        Assert.Equal(recordPeriod.Id, snapshot.Id);
        Assert.Equal(period, snapshot.Period);
        Assert.Equal([_activityId], @event.ActivityIds);
        Assert.Equal([_tagId], @event.TagIds);
    }

    [Fact]
    public void RunningRecord_ShouldBuildStartedEventWithEntityRefByUser()
    {
        var activities = new RelatedActivityIdsCollection(_activityId);
        var tags = new RelatedTagIdsCollection();
        var record = new RunningRecord(_userId, activities, tags, DateTimeOffset.UtcNow);

        var @event = record.BuildStartedEvent(EventId.Generate());

        Assert.Equal(EventTypes.RunningRecords.Started, @event.Type);
        Assert.Equal(new EntityRef(EntityType.RunningRecord, _userId.ToString()), @event.EntityRef);
        Assert.Equal(record.StartAt, @event.StartAt);
        Assert.Equal([_activityId], @event.ActivityIds);
        Assert.Empty(@event.TagIds);
    }

    [Fact]
    public void User_ShouldBuildRegisteredAndSettingsChangedEvents()
    {
        var nickname = Nickname.Parse("nickname", null);
        var user = new User(_userId, nickname);
        user.Settings.DayUtcOffset = new DayUtcOffset(TimeSpan.FromHours(3));

        var registered = user.BuildRegisteredEvent(EventId.Generate());
        var changed = user.BuildSettingsChangedEvent(EventId.Generate());

        Assert.Equal(EventTypes.Users.Registered, registered.Type);
        Assert.Equal(new EntityRef(EntityType.User, _userId.ToString()), registered.EntityRef);
        Assert.Equal(nickname, registered.Nickname);
        Assert.Equal(EventTypes.Users.SettingsChanged, changed.Type);
        Assert.Equal(TimeSpan.FromHours(3), changed.DayUtcOffset.Value);
    }
}
