using Freem.Web.Api.Public.FunctionalTests.Context;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Tests.Base;

[Collection("Tests")]
public abstract class TestBase : IDisposable
{
    protected TestContext Context { get; }

    protected TestBase(TestContext context, ITestOutputHelper? output = null)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        Context = context;
        Context.Output = output;
        
        Context.CleanDatabase();
    }

    public void Dispose()
    {
        Context.TokenLoader.Clean();
        Context.CleanDatabase();
        
        GC.SuppressFinalize(this);
    }
}