using Freem.UseCases.Abstractions;
using Freem.UseCases.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.UseCases.Implementations;

public sealed class UseCaseExecutor<TContext> : IUseCaseExecutor<TContext>
    where TContext : class
{
    private readonly UseCasesTypesCollection _types;
    private readonly IServiceProvider _services;

    public UseCaseExecutor(UseCasesTypesCollection types, IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(types);
        ArgumentNullException.ThrowIfNull(services);

        _types = types;
        _services = services;
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        TContext context, TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : notnull
    {
        var contextType = typeof(TContext);
        var requestType = typeof(TRequest);

        if (!_types.TryGetInvoker(contextType, requestType, out var invoker))
            throw new InvalidOperationException();

        var descriptor = invoker.Descriptor;
        var useCaseType = descriptor.AbstractionType;
        var useCase = _services.GetRequiredService(useCaseType);

        var response = await invoker.ExecuteAsync(useCase, context, request, cancellationToken);
        var responseType = response.GetType();

        if (responseType != descriptor.Arguments.ResponseType)
            throw new InvalidOperationException();

        return (TResponse)response;
    }
}
