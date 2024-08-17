using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions;

internal static class ExceptionAssert
{
    public static void TrueForFirstInner<TException, TInnerException>(TException exception, Predicate<TInnerException> predicate)
        where TException : Exception
        where TInnerException : Exception
    {
        Exception? e = exception;
        while (e is not null && e is not TInnerException)
            e = e.InnerException;

        if (e is null)
            Assert.Fail($"Can't get exception of type {nameof(TInnerException)}");

        var innerException = (TInnerException)e;
        var result = predicate(innerException);
        Assert.True(result);
    }
}
