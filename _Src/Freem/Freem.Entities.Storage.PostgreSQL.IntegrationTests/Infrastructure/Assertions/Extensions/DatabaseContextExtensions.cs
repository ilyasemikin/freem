using Freem.Entities.Storage.PostgreSQL.Database;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Freem.Entities.Storage.PostgreSQL.IntegrationTests.Infrastructure.Assertions.Extensions;

internal static class DatabaseContextExtensions
{
    public static async Task ShouldThrowExceptionAsync<TException>(this DatabaseContext context, Predicate<TException>? exceptionPredicate = null)
        where TException : Exception
    {
        var exception = await Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.NotNull(exception);
        Assert.IsType<TException>(exception);

        if (exception is TException concreteException && exceptionPredicate is not null)
            Assert.True(exceptionPredicate(concreteException));
    }

    public static async Task ShouldThrowExceptionAsync<TException, TInnerException>(this DatabaseContext context, Predicate<TInnerException> innerExceptionPredicate)
        where TException : Exception
        where TInnerException : Exception
    {
        var exception = await Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.NotNull(exception);
        Assert.IsType<TException>(exception);

        ExceptionAssert.TrueForFirstInner(exception, innerExceptionPredicate);
    }

    public static async Task ShouldNotThrowExceptionAsync(this DatabaseContext context)
    {
        var exception = await Record.ExceptionAsync(() => context.SaveChangesAsync());

        Assert.Null(exception);
    }
}
