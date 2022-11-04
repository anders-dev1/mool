using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Services;
using Application.Services.Session;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Features
{
    public record AuthenticationQuery(string Email, string Password) : IRequest<AuthenticationResult>;
    public record AuthenticationResult(AuthenticatedTokens Tokens);
    
    public class AuthenticationQueryValidator : AbstractValidator<AuthenticationQuery>
    {
        public AuthenticationQueryValidator()
        {
            RuleFor(e => e.Email).EmailAddress();
            RuleFor(e => e.Password).NotEmpty();
        }
    }
    
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, AuthenticationResult>
    {
        private readonly IMongoCollection<User> _users;
        private readonly IPasswordIssuer _passwordIssuer;
        private readonly IAuthenticatedTokensCreator _authenticatedTokensCreator;

        public AuthenticationQueryHandler(
            IMongoCollection<User> users,
            IPasswordIssuer passwordIssuer,
            IAuthenticatedTokensCreator authenticatedTokensCreator)
        {
            _users = users;
            _passwordIssuer = passwordIssuer;
            _authenticatedTokensCreator = authenticatedTokensCreator;
        }

        public async Task<AuthenticationResult> Handle(AuthenticationQuery query, CancellationToken cancellationToken)
        {
            var user = await _users
                .AsQueryable()
                .FirstOrDefaultAsync(e => e.Email == query.Email, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User was not found");
            }

            bool passwordMatch = _passwordIssuer.Verify(query.Password, user.Password);
            if (passwordMatch == false)
            {
                throw new UnauthorizedException("Password does not match the user's email");
            }
            
            var tokens = await _authenticatedTokensCreator.Create(user.Id.ToString());
            
            return new AuthenticationResult(tokens);
        }
    }
}