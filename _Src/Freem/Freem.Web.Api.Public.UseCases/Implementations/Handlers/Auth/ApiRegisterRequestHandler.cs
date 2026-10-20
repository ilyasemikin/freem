using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Identities;
using Freem.Domain.Users;
using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Auth;

internal sealed class ApiRegisterRequestHandler : BaseAnonymousRequestHandler<ApiRegisterRequest>
{
    private readonly IPasswordIdentityClient _client;

    public ApiRegisterRequestHandler(IPasswordIdentityClient client)
    {
        ArgumentNullException.ThrowIfNull(client);

        _client = client;
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAnonymousHandlerContext context, ApiRegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var egoHubRequest = new RegisterPasswordIdentityRequest
        {
            Username = request.Body.Username,
            Password = request.Body.Password
        };

        var result = await _client.RegisterAsync(egoHubRequest, cancellationToken);
        if (!result.Success)
            return EgoHubAuthSupport.MapError(result.Error);

        var userIdValue = result.Value.UserId.ToString();
        var userId = UserId.Parse(userIdValue, null);
        var response = new RegisterResponse(userId);
        return new OkObjectResult(response);
    }
}
