using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Net;

namespace Writings.Api.Tests.Integration.WritingsController
{
    [Collection("WritingsApi Collection")]
    public class GetWritingsControllerTests(WritingsApiFactory appFactory)
    {
        private readonly HttpClient _httpClient = appFactory.CreateClient();

        [Fact]
        public async Task Get_ShouldReturnNotFound_WhenWritingDoesNotExist()
        {
            //Act
            var response = await _httpClient.GetAsync($"api/writings/{Guid.NewGuid()}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem!.Title.Should().Be("Not Found");
            problem.Status.Should().Be(404);
        }
    }
}
