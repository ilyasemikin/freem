using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;

[Collection("Sequential")]
public abstract class ConstraintTriggerInTransactionTestsBase : IDisposable
{
    private readonly DatabaseContextHandle _handle;
    private readonly IDbContextTransaction _transaction;

    internal DatabaseContext Context => _handle.Context;

    protected ConstraintTriggerInTransactionTestsBase(ITestOutputHelper output)
    {
        _handle = DatabaseContextFactory.Create(output.WriteLine);
        Context.TruncateTables();
        
        _transaction = Context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();

        Context.TruncateTables();
        _handle.Dispose();
        
        GC.SuppressFinalize(this);
    }
}
