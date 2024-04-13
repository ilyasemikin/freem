﻿using Freem.Collections.Identifiers;
using Freem.DateTimePeriods;
using Freem.Entities.Constants;
using Freem.Entities.Helpers;

namespace Freem.Entities;

public class Record
{
    public const int MaxNameLength = LengthLimits.RecordMaxNameLength;
    public const int MaxDescriptionLength = LengthLimits.RecordMaxDescriptionLength;

    private string? _name;
    private string? _description;

    public string Id { get; }
    public string UserId { get; }
    public IdentifiersCollection CategoryIds { get; }
    public IdentifiersCollection TagIds { get; }

    public string? Name
    {
        get => _name;
        set
        {
            if (value?.Length > MaxNameLength)
                throw new ArgumentException($"Length cannot be more than {MaxNameLength}", nameof(value));

            _name = value;
        }
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (value?.Length > MaxDescriptionLength)
                throw new ArgumentException($"Length cannot be more than {MaxDescriptionLength}", nameof(value));

            _description = value;
        }
    }

    public DateTimePeriod Period { get; }

    public Record(
        string id,
        string userId,
        IEnumerable<string> categoryIds,
        IEnumerable<string>? tagIds,
        DateTimePeriod timePeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(categoryIds);
        ArgumentNullException.ThrowIfNull(timePeriod);

        Id = id;
        UserId = userId;
        
        CategoryIds = new IdentifiersCollection(categoryIds, IdentifiersCheckerStrategies.CategoryIdsCheckerStrategy);
        TagIds = new IdentifiersCollection(tagIds, IdentifiersCheckerStrategies.TagIdsCheckerStrategy);
        
        Period = timePeriod;
    }
}
