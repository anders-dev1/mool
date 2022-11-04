using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Domain;
using Application.Exceptions;
using Application.Features;
using Application.Services;
using AutoFixture;
using MongoDB.Driver;
using Moq;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Application.Features
{
    public class UserCreationCommandHandlerTests : UnitTest<UserCreationCommandHandlerTests>, IAsyncLifetime
    {
        // Collections
        private readonly IMongoCollection<User> _userCollection = Mongo.GetCollection<User>();

        // Services
        private readonly Mock<IPasswordIssuer> _passwordHandler = new Mock<IPasswordIssuer>();

        // sut
        private readonly UserCreationCommandHandler _userCreationCommandHandler;

        public UserCreationCommandHandlerTests()
        {
            _userCreationCommandHandler = new UserCreationCommandHandler(
                _userCollection,
                _passwordHandler.Object);
        }

        public async Task InitializeAsync() => await DisposeAsync();
        public async Task DisposeAsync()
        {
            await _userCollection.DeleteManyAsync(FilterDefinition<User>.Empty);
        }
        
        [Fact]
        public async Task Handle_WhenEverythingIsOk_AddsUserToDatabase()
        {
            // Arrange
            var password = "password";
            _passwordHandler.Setup(e => e.Hash(It.IsAny<string>())).Returns(password);
            
            var command = Fixture.Create<UserCreationCommand>() with{Email = "userfromfirsttest"};
            
            // Act
            await _userCreationCommandHandler.Handle(command, CancellationToken.None);
            var result = await _userCollection.Find(FilterDefinition<User>.Empty).ToListAsync();
            
            // Assert
            var createdUser = result.Single();
            
            Assert.Equal(command.Email, createdUser.Email);
            Assert.Equal(createdUser.Password, password);
            Assert.Equal(command.FirstName, createdUser.FirstName);
            Assert.Equal(command.LastName, createdUser.LastName);
        }
        
        [Fact]
        public async Task Handle_WhenEmailAlreadyExists_ThrowsConflict()
        {
            // Arrange
            var user = Fixture.Create<User>() with{ Email = "userfromothertest"};
            await _userCollection.InsertOneAsync(user);

            var command = Fixture.Create<UserCreationCommand>() with
            {
                Email = user.Email
            };

            // Act + Assert
            await Assert.ThrowsAsync<ConflictException>(
                () => _userCreationCommandHandler.Handle(command, CancellationToken.None));
        }
    }
}