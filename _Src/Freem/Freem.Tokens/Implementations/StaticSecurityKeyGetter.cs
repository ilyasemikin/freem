using System.Text;
using Freem.Tokens.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Tokens.Implementations;

public sealed class StaticSecurityKeyGetter : ISecurityKeyGetter
{
    private readonly SymmetricSecurityKey _key;

    public StaticSecurityKeyGetter(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        _key = new SymmetricSecurityKey(bytes);
    }

    public SecurityKey Get()
    {
        return _key;
    }
}