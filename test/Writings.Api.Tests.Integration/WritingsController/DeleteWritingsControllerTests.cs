using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Writings.Contracts.Requests;
using Writings.Contracts.Responses.V1;
using Bogus;
using Writings.Contracts.Requests.V1;
using Writings.Contracts.Enums;
using FluentAssertions;

namespace Writings.Api.Tests.Integration.WritingsController
{
    [Collection("WritingsApi Collection")]
    public class DeleteWritingsControllerTests(WritingsApiFactory appFactory)
    {
        private readonly HttpClient _httpClient = appFactory.CreateClient();

        private readonly Faker<CreateWritingRequest> _writingGenerator = new Faker<CreateWritingRequest>()
            .RuleFor(w => w.Title, faker => faker.Random.Word())
            .RuleFor(w => w.Body, faker => faker.Random.Words(20))
            .RuleFor(w => w.Type, WritingType.Original)
            .RuleFor(w => w.YearOfCompletion, faker => faker.Date.Past(1).Year);

        [Fact]
        public async Task Delete_ShouldReturnOkStatus_WhenWritingIsDeletedAndAuthorisationIsValid()
        {
            //Arrange
            var writing = _writingGenerator.Generate();

            var tokenGenerationRequest = new TokenGenerationRequest
            {
                UserId = Guid.NewGuid(),
                Email = "test@email.com",
                CustomClaims = new Dictionary<string, object>
                {
                    { "admin", true },
                    { "trusted_member", true }
                }
            };

            var tokenResponse = await _httpClient.PostAsJsonAsync("token", tokenGenerationRequest);
            var token = await tokenResponse.Content.ReadAsStringAsync();

            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var createdWritingReponse = await _httpClient.PostAsJsonAsync($"api/writings", writing);

            var createdWriting = await createdWritingReponse.Content.ReadFromJsonAsync<WritingResponse>();

            //Act
            var response = await _httpClient.DeleteAsync($"api/writings/{createdWriting!.Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
