using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Services;
using FluentValidation;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Features
{
    public record UserCreationCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Unit>;

    public class UserCreationCommandValidator : AbstractValidator<UserCreationCommand>
    {
        public UserCreationCommandValidator()
        {
            RuleFor(e => e.Email).EmailAddress();
            RuleFor(e => e.Password).MinimumLength(8).NotEmpty();
            RuleFor(e => e.FirstName).NotEmpty();
            RuleFor(e => e.LastName).NotEmpty();
        }
    }
    
    public class UserCreationCommandHandler : IRequestHandler<UserCreationCommand>
    {
        private readonly IMongoCollection<Domain.User> _users;
        private readonly IPasswordIssuer _passwordIssuer;

        public UserCreationCommandHandler(
            IMongoCollection<Domain.User> users,
            IPasswordIssuer passwordIssuer)
        {
            _users = users;
            _passwordIssuer = passwordIssuer;
        }

        public async Task<Unit> Handle(UserCreationCommand command, CancellationToken cancellationToken)
        {
            var existingUser = await _users
                .AsQueryable()
                .FirstOrDefaultAsync(e => e.Email == command.Email, cancellationToken: cancellationToken);
            if (existingUser != null)
            {
                throw new ConflictException("A user with that email already exists in the system.");
            }

            var hashedPassword = _passwordIssuer.Hash(command.Password);
            
            var user = new Domain.User(
                command.FirstName,
                command.LastName,
                command.Email,
                hashedPassword);
            
            await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
            
            return Unit.Value;
        }
    }
}