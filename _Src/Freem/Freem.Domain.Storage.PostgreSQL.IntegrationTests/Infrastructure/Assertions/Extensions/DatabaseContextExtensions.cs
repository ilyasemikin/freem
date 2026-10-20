using Freem.Domain.Storage.PostgreSQL.Database;
using Xunit;

namespace Freem.Domain.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;

internal static class DatabaseContextExtensions
{
    public static async Task ShouldThrowExceptionAsync<TException>(this DatabaseContext context, Predicate<TException>? exceptionPredicate = null)
        where TException : Exception
    {
        var exception = await Xunit.Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.NotNull(exception);

        // EF Core may wrap the provider exception (DbUpdateException), depending on the moment it is thrown
        var concreteException = FindException<TException>(exception);
        Assert.NotNull(concreteException);

        if (exceptionPredicate is not null)
            Assert.True(exceptionPredicate(concreteException));
    }

    public static async Task ShouldThrowExceptionAsync<TException, TInnerException>(this DatabaseContext context, Predicate<TInnerException> innerExceptionPredicate)
        where TException : Exception
        where TInnerException : Exception
    {
        var exception = await Xunit.Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.NotNull(exception);
        Assert.IsType<TException>(exception);

        ExceptionAssert.TrueForFirstInner(exception, innerExceptionPredicate);
    }

    public static async Task ShouldNotThrowExceptionAsync(this DatabaseContext context)
    {
        var exception = await Xunit.Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.Null(exception);
    }

    private static TException? FindException<TException>(Exception exception)
        where TException : Exception
    {
        Exception? current = exception;
        while (current is not null && current is not TException)
            current = current.InnerException;

        return current as TException;
    }
}
