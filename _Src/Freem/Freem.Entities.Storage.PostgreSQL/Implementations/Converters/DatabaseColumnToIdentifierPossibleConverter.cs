using System.Diagnostics.CodeAnalysis;
using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Models;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Converters;

internal sealed class DatabaseColumnToIdentifierPossibleConverter 
    : IPossibleConverter<DatabaseColumnWithValue, IEntityIdentifier>
{
    private delegate IEntityIdentifier EntityIdentifierFactory(string value);

    private readonly IReadOnlyDictionary<string, EntityIdentifierFactory> _factories =
        new Dictionary<string, EntityIdentifierFactory>()
        {
            [EntitiesNames.Activities.Table] = value => new ActivityIdentifier(value),
            [EntitiesNames.Records.Table] = value => new RecordIdentifier(value),
            [EntitiesNames.Tags.Table] = value => new TagIdentifier(value),
            [EntitiesNames.Users.Table] = value => new UserIdentifier(value)
        };

    public bool TryConvert(DatabaseColumnWithValue input, [NotNullWhen(true)] out IEntityIdentifier? output)
    {
        output = null;
        if (!_factories.TryGetValue(input.Column.Table, out var factory))
            return false;
        
        output = factory(input.Value);
        return true;
    }
}