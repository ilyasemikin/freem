using System.Diagnostics.CodeAnalysis;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Domain.Storage.PostgreSQL.Implementations.Converters;
using Utilities.Errors;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DatabaseForeignKeyConstraintErrorToErrorConverter
{
    private readonly DatabaseColumnToEntityNotFoundErrorConverter _columnConverter;

    public DatabaseForeignKeyConstraintErrorToErrorConverter(
        DatabaseColumnToEntityNotFoundErrorConverter columnConverter)
    {
        _columnConverter = columnConverter;
    }

    public bool TryConvert(DatabaseForeignKeyConstraintError error, [NotNullWhen(true)] out Error? output)
    {
        output = null;
        if (!_columnConverter.TryConvert(error.Column, out var notFound))
            return false;

        output = DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([notFound]);
        return true;
    }
}