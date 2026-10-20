using Freem.Domain.Users.Models;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Users.Register;

public class RegisterUserRequest : IRequest
{
    public RequestType Type => RequestTypes.Users.Register;

    public Nickname Nickname { get; }

    public RegisterUserRequest(Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);
        
        Nickname = nickname;
    }
}
