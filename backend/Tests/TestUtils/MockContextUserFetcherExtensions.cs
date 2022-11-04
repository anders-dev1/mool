using Application.Domain;
using Application.Services.Session;
using AutoFixture;
using Moq;
using Tests.TestUtils.Bootstrapping;

namespace Tests.TestUtils
{
    public static class MockContextUserFetcherExtensions
    {
        public static User SeedUser(this Mock<IContextUserFetcher> mock)
        {
            var fixture = CustomFixtureCreator.Create();
            var user = fixture.Create<User>();
            
            mock.Setup(e => e.Fetch()).Returns(user);
            mock.Setup(e => e.FetchOrThrow()).Returns(user);

            return user;
        } 
    }
}