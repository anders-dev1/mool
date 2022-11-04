using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Services.Session;
using Application.Services.Session.Model;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Features
{
    public record RenewAccessTokenCommand(string RefreshToken) : IRequest<RenewAccessTokenResult>;
    public record RenewAccessTokenResult(string AccessToken);
    
    public class RenewAccessTokenCommandHandler : IRequestHandler<RenewAccessTokenCommand, RenewAccessTokenResult>
    {
        private readonly IMongoCollection<RefreshToken> _refreshTokens;
        private readonly IAccessTokenCreator _accessTokenCreator;

        public RenewAccessTokenCommandHandler( 
            IMongoCollection<RefreshToken> refreshTokens, 
            IAccessTokenCreator accessTokenCreator)
        {
            _refreshTokens = refreshTokens;
            _accessTokenCreator = accessTokenCreator;
        }

        public async Task<RenewAccessTokenResult> Handle(RenewAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var parseSuccess = ObjectId.TryParse(request.RefreshToken, out var refreshTokenId);
            if (parseSuccess == false)
            {
                throw new BadRequestException("Refresh token was in the wrong format.");
            }

            var refreshTokenResult = await _refreshTokens.FindAsync(
                e => e.Id == refreshTokenId && e.Expires > DateTimeOffset.Now, cancellationToken: cancellationToken);
            var refreshToken = refreshTokenResult.SingleOrDefault();
            if (refreshToken == null)
            {
                throw new NotFoundException("Requested refresh token has expired or been revoked.");
            }

            var accessToken = _accessTokenCreator.Create(refreshToken.UserId.ToString());
            return new RenewAccessTokenResult(accessToken);
        }
    }
}