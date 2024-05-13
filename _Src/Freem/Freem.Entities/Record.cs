﻿using Freem.DateTimePeriods;
using Freem.Entities.Abstractions;
using Freem.Entities.Collections;
using Freem.Entities.Collections.Abstractions;
using Freem.Entities.Constants;

namespace Freem.Entities;

public class Record : IEntity
{
    public const int MaxNameLength = LengthLimits.RecordMaxNameLength;
    public const int MaxDescriptionLength = LengthLimits.RecordMaxDescriptionLength;

    private string? _name;
    private string? _description;

    public string Id { get; }
    public string UserId { get; }
    public RelatedCategoriesCollection Categories { get; }
    public RelatedTagsCollection Tags { get; }

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

    public DateTimePeriod Period { get; set; }

    public Record(
        string id,
        string userId,
        RelatedCategoriesCollection categories,
        RelatedTagsCollection tags,
        DateTimePeriod timePeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(timePeriod);

        Id = id;
        UserId = userId;
        
        Categories = categories;
        Tags = tags;
        
        Period = timePeriod;
    }
}
