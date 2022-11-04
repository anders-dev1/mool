using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Session
{
    public interface IAccessTokenCreator
    {
        string Create(string userId);
    }
    
    public class AccessTokenCreator : IAccessTokenCreator
    {
        private readonly IJwtSettings _jwtSettings;
        private readonly IEnvironmentSettings _environmentSettings;

        public AccessTokenCreator(
            IJwtSettings jwtSettings,
            IEnvironmentSettings environmentSettings)
        {
            _jwtSettings = jwtSettings;
            _environmentSettings = environmentSettings;
        }
        
        public string Create(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Issuer = _environmentSettings.Url,
                Expires = DateTime.UtcNow.AddHours(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var serializedToken = tokenHandler.WriteToken(token);

            return serializedToken;
        }
    }
}