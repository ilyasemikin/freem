using System.Text;
using Freem.Tokens.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Tokens.Implementations;

public sealed class StaticSecurityKeyGetter : ISecurityKeyGetter
{
    private readonly string _value;

    public StaticSecurityKeyGetter(string value)
    {
        _value = value;
    }

    public SecurityKey Get()
    {
        var bytes = Encoding.UTF8.GetBytes(_value);
        return new SymmetricSecurityKey(bytes);
    }
}