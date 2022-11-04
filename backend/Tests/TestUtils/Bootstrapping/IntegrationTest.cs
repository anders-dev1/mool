using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using API;
using Application.Services;
using Application.Settings;
using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shared;
using Xunit;

namespace Tests.TestUtils.Bootstrapping;

/// <summary>
/// Base integration test class for testing how two or more parts of the solution work together.
/// Bootstraps a new database for the integration tests and sets up a mock IUserFetcher for use with ApiUserMocker.
/// </summary>
/// <typeparam name="T">Integration test class, used to create unique database</typeparam>
public class IntegrationTest<T> :
    WebApplicationFactory<Startup>,
    IAsyncLifetime
{
    protected static readonly MongoAccessor Mongo = new(typeof(T).Name);
    public IFixture Fixture { get; } = new Fixture().Customize(new FixtureCustomization());
    public readonly Mock<IUserFetcher> UserFetcherMock;
    public readonly HttpClient TestClient;

    protected IntegrationTest()
    {
        UserFetcherMock = new Mock<IUserFetcher>();
        TestClient = CreateClient();
    }

    public async Task InitializeAsync() => await DisposeAsync();
    public async Task DisposeAsync() => await Mongo.DropDatabase();

    // Makes it possible to overwrite services using the ConfigureServices virtual method.
    protected virtual void ConfigureServices(IServiceCollection serviceCollection) { }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(UserFetcherMock.Object);
            ConfigureServices(services);
        });

        builder.ConfigureAppConfiguration((configurationBuilder =>
        {
            configurationBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>($"{nameof(AppSettings.MongoDb)}:{nameof(MongoDbSettings.DatabaseName)}", typeof(T).Name)
            });
        }));

        builder.UseEnvironment("Development");
    }
}