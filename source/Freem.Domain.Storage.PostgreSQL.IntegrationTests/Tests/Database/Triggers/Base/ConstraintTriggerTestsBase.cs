using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;

[Collection("Sequential")]
public abstract class ConstraintTriggerTestsBase : IDisposable
{
    private readonly DatabaseContextHandle _handle;

    internal DatabaseContext Context => _handle.Context;

    protected ConstraintTriggerTestsBase(ITestOutputHelper output)
    {
        _handle = DatabaseContextFactory.Create(output.WriteLine);
        Context.TruncateTables();
    }

    public void Dispose()
    {
        Context.TruncateTables();
        _handle.Dispose();
        
        GC.SuppressFinalize(this);
    }
}
