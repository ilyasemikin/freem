using System.Diagnostics.CodeAnalysis;
using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Models;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Converters;

internal sealed class DatabaseColumnToIdentifierPossibleConverter 
    : IPossibleConverter<DatabaseColumnWithValue, IEntityIdentifier>
{
    private delegate IEntityIdentifier EntityIdentifierFactory(string value);

    private readonly IReadOnlyDictionary<DatabaseColumn, EntityIdentifierFactory> _factories =
        new Dictionary<DatabaseColumn, EntityIdentifierFactory>()
        {
            [new DatabaseColumn(EntitiesNames.Categories.Table, EntitiesNames.Categories.Properties.Id)] = value => new CategoryIdentifier(value),
            [new DatabaseColumn(EntitiesNames.Records.Table, EntitiesNames.Records.Properties.Id)] = value => new RecordIdentifier(value),
            [new DatabaseColumn(EntitiesNames.Tags.Table, EntitiesNames.Tags.Properties.Id)] = value => new TagIdentifier(value),
            [new DatabaseColumn(EntitiesNames.Users.Table, EntitiesNames.Users.Properties.Id)] = value => new UserIdentifier(value)
        };

    public bool TryConvert(DatabaseColumnWithValue input, [NotNullWhen(true)] out IEntityIdentifier? output)
    {
        output = null;
        if (!_factories.TryGetValue(input.Column, out var factory))
            return false;
        
        output = factory(input.Value);
        return true;
    }
}