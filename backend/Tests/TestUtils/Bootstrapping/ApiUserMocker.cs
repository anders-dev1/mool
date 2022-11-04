using System.Net.Http.Headers;
using Application.Domain;
using Application.Services.Session;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Moq;

namespace Tests.TestUtils.Bootstrapping
{
    public static class ApiUserMocker
    {
        /// <summary>
        /// Spoofs a basic user to be used by the webapplication. Adds token to client.
        /// </summary>
        /// <param name="webApplicationTest"></param>
        /// <returns></returns>
        public static UserMock Seed<T>(IntegrationTest<T> webApplicationTest)
        {
            var userId = ObjectId.GenerateNewId();
            
            // Adds user to IUserFetcher mock to be used in UserContextMiddleware.
            var user = webApplicationTest.Fixture.Build<User>()
                .With(e => e.Id, userId)
                .Create();
            webApplicationTest.UserFetcherMock.Setup(e => e.Fetch(userId)).ReturnsAsync(user);
            
            // Get token and store in client.
            using var scope = webApplicationTest.Services.CreateScope();
            var jwtTokenCreator = scope.ServiceProvider.GetRequiredService<IAccessTokenCreator>();
            var token = jwtTokenCreator.Create(userId.ToString());
            webApplicationTest.TestClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            return new UserMock(user, token);
        }
    }

    public class UserMock
    {
        public User User { get; }
        public string Token { get; }

        public UserMock(User user, string token)
        {
            User = user;
            Token = token;
        }
    }
}