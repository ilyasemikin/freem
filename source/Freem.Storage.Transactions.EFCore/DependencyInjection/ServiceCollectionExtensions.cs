using Freem.Storage.Transactions.Abstractions;
using Freem.Storage.Transactions.EFCore.Implementations;
using Freem.Storage.Transactions.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Storage.Transactions.EFCore.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageTransactions<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.TryAddScoped<IStorageTransactionFactory, ContextStorageTransactionFactory<TDbContext>>();
        services.TryAddScoped<StorageTransactionRunner>();
        
        return services;
    }
}