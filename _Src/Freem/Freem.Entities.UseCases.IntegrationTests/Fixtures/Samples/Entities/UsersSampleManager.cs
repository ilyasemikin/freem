using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

public sealed class UsersSampleManager
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string Password = "password";
    
    private readonly ServicesContext _services;

    public UsersSampleManager(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public UserIdentifier Register()
    {
        var request = new RegisterUserPasswordRequest(Nickname, Login, Password);

        var response = _services.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(
            UseCaseExecutionContext.Empty, 
            request);

        return response.UserId;
    }
}