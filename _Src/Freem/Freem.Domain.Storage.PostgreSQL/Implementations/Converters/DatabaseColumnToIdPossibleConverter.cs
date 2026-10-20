using System.Diagnostics.CodeAnalysis;
using Freem.Domain;
using Freem.Domain.Activities;
using Freem.Domain.Records;
using Freem.Domain.Tags;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Models;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Converters;

internal sealed class DatabaseColumnToIdPossibleConverter
{
    private delegate Id EntityIdFactory(string value);

    private readonly IReadOnlyDictionary<string, EntityIdFactory> _factories =
        new Dictionary<string, EntityIdFactory>()
        {
            [EntitiesNames.Activities.Table] = value => ActivityId.Parse(value, null),
            [EntitiesNames.Records.Table] = value => RecordId.Parse(value, null),
            [EntitiesNames.Tags.Table] = value => TagId.Parse(value, null),
            [EntitiesNames.Users.Table] = value => UserId.Parse(value, null)
        };

    public bool TryConvert(DatabaseColumnWithValue input, [NotNullWhen(true)] out Id? output)
    {
        output = null;
        if (!_factories.TryGetValue(input.Column.Table, out var factory))
            return false;
        
        output = factory(input.Value);
        return true;
    }
}