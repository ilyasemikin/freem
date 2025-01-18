using Freem.Entities.UseCases.Contracts.Users.Password.Register;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures.Samples.Entities;

public sealed class UsersSampleManager
{
    private const string Login = "user";
    private const string Password = "password";
    
    private readonly ServicesContext _services;

    public UsersSampleManager(ServicesContext services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public UserIdentifier Register(string login = Login)
    {
        var request = new RegisterUserPasswordRequest(login, login, Password);

        var response = _services.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(
            UseCaseExecutionContext.Empty, 
            request);

        return response.UserId;
    }
}