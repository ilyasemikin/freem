using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.IntegrationTests.Fixtures;
using Freem.Entities.UseCases.IntegrationTests.Tests.Abstractions;
using Freem.Entities.UseCases.Users.Password.Register.Models;
using Freem.Entities.UseCases.Users.Password.Update.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.IntegrationTests.Tests;

public sealed class UsersPasswordUpdateUseCaseTests : UseCaseTestBase
{
    private const string Nickname = "user";
    private const string Login = "user";
    private const string OldPassword = "oldPassword";
    private const string NewPassword = "newPassword";

    private readonly UseCaseExecutionContext _context;
    
    public UsersPasswordUpdateUseCaseTests(ServicesContext context) 
        : base(context)
    {
        var request = new RegisterUserPasswordRequest(Nickname, Login, OldPassword);
        var response = Context.RequestExecutor.Execute<RegisterUserPasswordRequest, RegisterUserPasswordResponse>(UseCaseExecutionContext.Empty, request);

        _context = new UseCaseExecutionContext(response.UserId);
    }

    [Fact]
    public async Task ShouldSuccess()
    {
        var request = new UpdateLoginCredentialsRequest(OldPassword, NewPassword);

        var response = await Context.RequestExecutor.ExecuteAsync<UpdateLoginCredentialsRequest, UpdateLoginCredentialsResponse>(_context, request);
        
        Assert.NotNull(response);
        Assert.True(response.Success);
    }
}