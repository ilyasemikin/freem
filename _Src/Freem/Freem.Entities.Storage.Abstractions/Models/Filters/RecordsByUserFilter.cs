﻿using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;
using Freem.Entities.Storage.Abstractions.Models.Filters.Models;

namespace Freem.Entities.Storage.Abstractions.Models.Filters;

public sealed class RecordsByUserFilter : ILimitFilter, IOffsetFilter
{
    public Limit Limit { get; init; }
    public Offset Offset { get; init; }
    
    public UserIdentifier UserId { get; } 
    
    public RecordsByUserFilter(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
    }
}