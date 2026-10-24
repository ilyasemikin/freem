using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Activities;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Records;
using Freem.Domain.Tags;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Constants;
using Freem.Domain.Storage.PostgreSQL.Database.Models;
using Utilities.Errors;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Converters;

internal sealed class DatabaseColumnToEntityNotFoundErrorConverter
{
    private delegate Error NotFoundErrorFactory(string value);

    private readonly IReadOnlyDictionary<string, NotFoundErrorFactory> _factories =
        new Dictionary<string, NotFoundErrorFactory>()
        {
            [EntitiesNames.Activities.Table] = value =>
            {
                var id = ActivityId.Parse(value, null);
                return DomainErrorCatalog.Factories.CreateActivityNotFound(id);
            },
            [EntitiesNames.ImportDocuments.Table] = value =>
            {
                var id = ImportDocumentId.Parse(value, null);
                return DomainErrorCatalog.Factories.CreateImportDocumentNotFound(id);
            },
            [EntitiesNames.Records.Table] = value =>
            {
                var id = RecordId.Parse(value, null);
                return DomainErrorCatalog.Factories.CreateRecordNotFound(id);
            },
            [EntitiesNames.Tags.Table] = value =>
            {
                var id = TagId.Parse(value, null);
                return DomainErrorCatalog.Factories.CreateTagNotFound(id);
            },
            [EntitiesNames.Users.Table] = value =>
            {
                var id = UserId.Parse(value, null);
                return DomainErrorCatalog.Factories.CreateUserNotFound(id);
            }
        };

    public bool TryConvert(DatabaseColumnWithValue input, [NotNullWhen(true)] out Error? output)
    {
        output = null;
        if (!_factories.TryGetValue(input.Column.Table, out var factory))
            return false;

        output = factory(input.Value);
        return true;
    }
}
