using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.TestUtils.Bootstrapping
{
    /// <summary>
    /// Bootstrapper meant for API tests.
    /// Overwrites the IMediator interface so no request gets to the application layer.
    /// </summary>
    public class ApiTest<T> : IntegrationTest<T>
    {
        protected Mock<IMediator> Mediator = null!;

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            Mediator = new Mock<IMediator>();
            serviceCollection.AddSingleton(Mediator.Object);
        }
    }
}