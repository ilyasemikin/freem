namespace Freem.Tokens.Abstractions;

public interface ITokensBlacklist
{
    Task AddAsync(string token, CancellationToken cancellationToken);
    
    Task<bool> ContainsAsync(string token, CancellationToken cancellationToken);
}
