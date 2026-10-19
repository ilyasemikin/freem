using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Tags.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Entities.Storage.PostgreSQL.Database.Models;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Converters;

internal sealed class DatabaseColumnToIdentifierPossibleConverter
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