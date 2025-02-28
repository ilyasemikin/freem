using Freem.Entities.Storage.Abstractions.Base.Search;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.Users;
using Freem.Entities.Users.Identifiers;
using Freem.Tokens.Abstractions;

namespace Freem.Entities.UseCases.Users.Tokens;

internal sealed class RefreshUserAccessTokenUseCase 
    : IEntitiesUseCase<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse, RefreshUserAccessTokenErrorCode>
{
    private readonly RefreshTokenValidator _refreshTokenValidator;
    private readonly ITokensBlacklist _tokensBlacklist;
    private readonly ISearchByIdRepository<User, UserIdentifier> _usersRepository;
    private readonly AccessTokenGenerator _accessTokenGenerator;
    private readonly RefreshTokenGenerator _refreshTokenGenerator;

    public RefreshUserAccessTokenUseCase(
        RefreshTokenValidator refreshTokenValidator,
        ITokensBlacklist tokensBlacklist,
        ISearchByIdRepository<User, UserIdentifier> usersRepository,
        AccessTokenGenerator accessTokenGenerator,
        RefreshTokenGenerator refreshTokenGenerator)
    {
        ArgumentNullException.ThrowIfNull(refreshTokenGenerator);
        ArgumentNullException.ThrowIfNull(tokensBlacklist);
        ArgumentNullException.ThrowIfNull(usersRepository);
        ArgumentNullException.ThrowIfNull(accessTokenGenerator);
        ArgumentNullException.ThrowIfNull(refreshTokenGenerator);
        
        _refreshTokenValidator = refreshTokenValidator;
        _tokensBlacklist = tokensBlacklist;
        _usersRepository = usersRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<RefreshUserAccessTokenResponse> ExecuteAsync(
        UseCaseExecutionContext context, RefreshUserAccessTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _refreshTokenValidator.ValidateAsync(request.RefreshToken, cancellationToken);
        if (!validationResult.IsValid)
            return RefreshUserAccessTokenResponse.CreateFailure(RefreshUserAccessTokenErrorCode.TokenInvalid);

        await _tokensBlacklist.AddAsync(request.RefreshToken, cancellationToken);

        var userId = validationResult.UserId;
        var result = await _usersRepository.FindByIdAsync(userId, cancellationToken);
        
        if (!result.Founded)
            return RefreshUserAccessTokenResponse.CreateFailure(RefreshUserAccessTokenErrorCode.UserNotFound);

        var user = result.Entity;

        var atp = new AccessTokenProperties(user.Id);
        var rtp = new RefreshTokenProperties(user.Id);
        
        var accessToken = _accessTokenGenerator.Generate(atp);
        var refreshToken = _refreshTokenGenerator.Generate(rtp);
        
        return RefreshUserAccessTokenResponse.CreateSuccess(accessToken, refreshToken);
    }
}