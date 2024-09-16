using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Database.Extensions;
using Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Tests.Database.Triggers.Base;

[Collection("Sequential")]
public abstract class ConstraintTriggerTestsBase : IDisposable
{
    internal DatabaseContext Context { get; }

    protected ConstraintTriggerTestsBase(ITestOutputHelper output)
    {
        Context = DatabaseContextFactory.Create(output.WriteLine);
        Context.TruncateTables();
    }

    public void Dispose()
    {
        Context.TruncateTables();
        Context.Dispose();
        
        GC.SuppressFinalize(this);
    }
}
