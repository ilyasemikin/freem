using Microsoft.IdentityModel.Tokens;

namespace Freem.Tokens.Abstractions;

public interface ISecurityKeyGetter
{
    SecurityKey Get();
}