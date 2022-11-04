using System.Threading.Tasks;

namespace Application.Services.Session
{
    public interface IAuthenticatedTokensCreator
    {
        Task<AuthenticatedTokens> Create(string authenticatedUserId);
    }
    
    public record AuthenticatedTokens(string AccessToken, string RefreshToken);

    /// <summary>
    /// This service returns both a new access token and new refresh token for the userId.
    /// </summary>
    public class AuthenticatedTokensCreator : IAuthenticatedTokensCreator
    {
        private readonly IAccessTokenCreator _accessTokenCreator;
        private readonly IRefreshTokenCreator _refreshTokenCreator;

        public AuthenticatedTokensCreator(
            IAccessTokenCreator accessTokenCreator,
            IRefreshTokenCreator refreshTokenCreator)
        {
            _accessTokenCreator = accessTokenCreator;
            _refreshTokenCreator = refreshTokenCreator;
        }

        public async Task<AuthenticatedTokens> Create(string authenticatedUserId)
        {
            return new AuthenticatedTokens(
                _accessTokenCreator.Create(authenticatedUserId),
                await _refreshTokenCreator.Create(authenticatedUserId));
        }
    }
}