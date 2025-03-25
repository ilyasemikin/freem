using System.Text;
using Freem.Credentials.Password.Abstractions;

namespace Freem.Credentials.Password.Implementations;

public sealed class GuidSaltGenerator : ISaltGenerator
{
    public byte[] Generate()
    {
        var value = Guid.NewGuid().ToString("N");
        return Encoding.UTF8.GetBytes(value);
    }
}