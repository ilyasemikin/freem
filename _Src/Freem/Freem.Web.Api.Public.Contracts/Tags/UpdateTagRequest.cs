﻿using Freem.Entities.Tags.Models;

namespace Freem.Web.Api.Public.Contracts.Tags;

public sealed class UpdateTagRequest
{
    public UpdateField<TagName>? Name { get; init; }

    public bool HasChanges()
    {
        return Name is not null;
    }
}