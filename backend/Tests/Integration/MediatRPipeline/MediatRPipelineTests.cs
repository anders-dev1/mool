using System.Net;
using Application.Domain;
using Application.Features;
using MongoDB.Driver;
using Tests.TestUtils;
using Tests.TestUtils.Bootstrapping;
using Xunit;

namespace Tests.Integration.MediatRPipeline;

/// <summary>
/// Tests if requests get caught by the validation pipeline and if they end up getting executed.
/// </summary>
public class MediatRPipelineTests : IntegrationTest<MediatRPipelineTests>
{
    [Fact]
    public async void ThreadCreationCommand_WhenInvalid_ShouldReturnBadRequest()
    {
        // Arrange
        ApiUserMocker.Seed(this);
        var command = new ThreadCreationCommand(string.Empty);

        // Act
        var response = await TestClient.PostAsync($"api/thread", command.ToJson());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async void ThreadCreationCommand_WhenValid_ShouldCreateThread()
    {
        // Arrange
        ApiUserMocker.Seed(this);
        var command = new ThreadCreationCommand("This string is more than 15 characters.");

        // Act
        var response = await TestClient.PostAsync($"api/thread", command.ToJson());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var thread = await Mongo.GetCollection<MoolThread>().AsQueryable().SingleOrDefaultAsync();
        Assert.Equal(command.Content, thread.Content);
    }
}