using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Executors;

public sealed class UsersExecutor
{
    private readonly IRequestMediator<HandlerContext> _mediator;

    public UsersExecutor(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    public async Task<UserId> RegisterAsync()
    {
        var userId = UserId.Generate();
        var context = new HandlerContext(userId);

        var value = Guid.NewGuid().ToString();
        var nickname = Nickname.Parse(value, null);
        var request = new RegisterUserRequest(nickname);

        var result = await _mediator.ExecuteAsync<RegisterUserRequest, Result<RegisterUserResponse, Error>>(context, request);
        var response = result.EnsureSuccess();
        return response.UserId;
    }
}