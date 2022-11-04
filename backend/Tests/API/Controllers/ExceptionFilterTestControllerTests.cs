using System;
using System.Net;
using System.Threading.Tasks;
using API.Controllers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tests.TestUtils;
using Xunit;

namespace Tests.API.Controllers
{
    public partial class ApiTests
    {
        [Fact]
        public async void Endpoints_ReturnsExpectedExceptionFilterResults()
        {
            await TestEndpoint(HttpStatusCode.BadRequest, "api/exceptionfiltertest/badrequest");
            await TestEndpoint(HttpStatusCode.Conflict, "api/exceptionfiltertest/conflict");
            await TestEndpoint(HttpStatusCode.NotFound, "api/exceptionfiltertest/notfound");
            await TestEndpoint(HttpStatusCode.Unauthorized, "api/exceptionfiltertest/unauthorized");
        }
        
        private async Task TestEndpoint(HttpStatusCode statusCode, string endpoint)
        {
            // Arrange
            var expectedError = Guid.NewGuid().ToString();
            var key = ExceptionFilterTestController.Key;
            var request = new ExceptionTestRequest(expectedError, key);
            
            // Act
            var response = await TestClient.PostAsync(endpoint, request.ToJson());
            
            // Assert
            Assert.Equal(statusCode, response.StatusCode);
            
            var message = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedError, message);
        }

        [Fact]
        public async void Validation_ReturnsExpectedValidationErrors()
        {
            // Arrange
            var validationErrors = new ValidationError[]
            {
                new ValidationError(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                new ValidationError(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                new ValidationError(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
            };
            var request = new ValidationTestRequest(
                validationErrors, 
                Guid.NewGuid().ToString(),
                ExceptionFilterTestController.Key);

            // Act
            var response = await TestClient.PostAsync("api/exceptionfiltertest/validation", request.ToJson());
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            var failures = JsonConvert.DeserializeObject<ValidationFailure[]>(responseString);
            for (int i = 0; i < failures.Length; i++)
            {
                var payloadError = validationErrors[i];
                var resultError = failures[i];
                
                Assert.Equal(payloadError.PropertyName, resultError.PropertyName);
                Assert.Equal(payloadError.Error, resultError.ErrorMessage);
            }
        }
    }
}