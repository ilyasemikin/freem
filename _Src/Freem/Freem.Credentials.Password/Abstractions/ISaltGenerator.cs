namespace Freem.Credentials.Password.Abstractions;

public interface ISaltGenerator
{
    byte[] Generate();
}