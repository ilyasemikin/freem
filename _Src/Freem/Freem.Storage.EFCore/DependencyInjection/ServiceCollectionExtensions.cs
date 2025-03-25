using Freem.Storage.Abstractions;
using Freem.Storage.Abstractions.Helpers;
using Freem.Storage.EFCore.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Storage.EFCore.DependencyInjection;

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