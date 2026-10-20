using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.RunningRecords.Start;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.RunningRecords;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Clock.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.RunningRecords;

internal sealed class ApiStartRunningRecordRequestHandler : BaseAuthorizedRequestHandler<ApiStartRunningRecordRequest>
{
    private readonly IClock _clock;

    public ApiStartRunningRecordRequestHandler(IRequestMediator<HandlerContext> mediator, IClock clock)
        : base(mediator)
    {
        ArgumentNullException.ThrowIfNull(clock);

        _clock = clock;
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiStartRunningRecordRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map(_clock);
        var response = await Mediator.ExecuteAsync<StartRunningRecordRequest, Result<StartRunningRecordResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
