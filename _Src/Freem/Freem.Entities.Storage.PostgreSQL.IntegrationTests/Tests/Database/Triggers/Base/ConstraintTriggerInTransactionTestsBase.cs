using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;

[Collection("Sequential")]
public abstract class ConstraintTriggerInTransactionTestsBase : IDisposable
{
    private readonly IDbContextTransaction _transaction;

    internal DatabaseContext Context { get; }

    protected ConstraintTriggerInTransactionTestsBase(ITestOutputHelper output)
    {
        Context = DatabaseContextFactory.Create(output.WriteLine);
        Context.TruncateTables();
        
        _transaction = Context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();

        Context.TruncateTables();
        Context.Dispose();
        
        GC.SuppressFinalize(this);
    }
}
