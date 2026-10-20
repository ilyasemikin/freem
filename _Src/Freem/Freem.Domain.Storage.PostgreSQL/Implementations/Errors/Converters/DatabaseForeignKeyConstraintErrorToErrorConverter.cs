using System.Diagnostics.CodeAnalysis;
using Freem.Domain;
using Freem.Domain.Storage.PostgreSQL.Database.Errors.Implementations;
using Freem.Domain.Storage.PostgreSQL.Implementations.Converters;
using Utilities.Errors;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Errors.Converters;

internal sealed class DatabaseForeignKeyConstraintErrorToErrorConverter
{
    private readonly DatabaseColumnToIdPossibleConverter _columnToIdConverter;

    public DatabaseForeignKeyConstraintErrorToErrorConverter(
        DatabaseColumnToIdPossibleConverter columnToIdConverter)
    {
        _columnToIdConverter = columnToIdConverter;
    }

    public bool TryConvert(DatabaseForeignKeyConstraintError error, [NotNullWhen(true)] out Error? output)
    {
        output = null;
        if (!_columnToIdConverter.TryConvert(error.Column, out var id))
            return false;

        output = DomainErrorCatalog.Factories.CreateRelatedEntitiesNotFound([id]);
        return true;
    }
}