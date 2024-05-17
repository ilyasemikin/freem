using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Triggers.Base;

[Collection("Sequential")]
public abstract class ConstraintTriggerInTransactionTestsBase : IDisposable
{
    private readonly IDbContextTransaction _transaction;

    internal DatabaseContext Context { get; }

    public ConstraintTriggerInTransactionTestsBase()
    {
        Context = DatabaseContextFactory.Create();
        _transaction = Context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();

        GC.SuppressFinalize(this);
    }
}
