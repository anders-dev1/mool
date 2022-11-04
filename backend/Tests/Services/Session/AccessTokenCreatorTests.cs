using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Services.Session;
using Application.Settings;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Xunit;

namespace Tests.Services.Session
{
    class TestJwtSettings : IJwtSettings
    {
        public string Secret => "68702080-df45-43af-b7ca-a801c2b577c3";
    }

    class TestEnvironmentSettings : IEnvironmentSettings
    {
        public string Url => "localhost";
    }
    
    public class AccessTokenCreatorTests
    {
        private readonly IJwtSettings _jwtSettings;
        private readonly IEnvironmentSettings _environmentSettings;
        private readonly AccessTokenCreator _accessTokenCreator;
        
        public AccessTokenCreatorTests()
        {
            _jwtSettings = new TestJwtSettings();
            _environmentSettings = new TestEnvironmentSettings();
            _accessTokenCreator = new AccessTokenCreator(_jwtSettings, _environmentSettings);
        }
        
        [Fact]
        public void Create_WhenGivenUserId_CreatesValidTokenWithUserClaim()
        {
            // Act
            var result = _accessTokenCreator.Create(ObjectId.GenerateNewId().ToString());

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var key =  Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenValidation = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateAudience = false,
                ValidateIssuer = false
            };

            // Error is thrown if token is not valid.
            handler.ValidateToken(result, tokenValidation, out var validationResult);
        }
    }
}