using Bogus;
using System.Net.Http.Json;
using System.Net;
using Writings.Contracts.Enums;
using Writings.Contracts.Requests;
using Writings.Contracts.Requests.V1;
using Writings.Contracts.Responses.V1;
using FluentAssertions;

namespace Writings.Api.Tests.Integration.WritingsController
{
    [Collection("WritingsApi Collection")]
    public class UpdateWritingsControllerTests(WritingsApiFactory appFactory) : IAsyncLifetime
    {
        private readonly HttpClient _httpClient = appFactory.CreateClient();

        private readonly Faker<CreateWritingRequest> _writingGenerator = new Faker<CreateWritingRequest>()
            .RuleFor(w => w.Title, faker => faker.Random.Word())
            .RuleFor(w => w.Body, faker => faker.Random.Words(20))
            .RuleFor(w => w.Type, WritingType.Original)
            .RuleFor(w => w.YearOfCompletion, faker => faker.Date.Past(1).Year);

        private readonly Faker<UpdateWritingRequest> _updateWritingRequestGenerator = new Faker<UpdateWritingRequest>()
            .RuleFor(w => w.Title, faker => faker.Random.Word())
            .RuleFor(w => w.Body, faker => faker.Random.Words(20))
            .RuleFor(w => w.Type, WritingType.Original)
            .RuleFor(w => w.YearOfCompletion, faker => faker.Date.Past(1).Year);

        private readonly List<Guid> _createdIds = [];

        [Fact]
        public async Task Update_ShouldReturnOkStatus_WhenWritingIsUpdatedAndAuthorisationIsValid()
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

            var updateWritingRequest = _updateWritingRequestGenerator.Generate();

            //Act
            var response = await _httpClient.PutAsJsonAsync($"api/writings/{createdWriting!.Id}", updateWritingRequest);

            //Assert
            var updateWritingResponse = await response.Content.ReadFromJsonAsync<WritingResponse>();
            updateWritingResponse.Should().BeEquivalentTo(updateWritingRequest);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _createdIds.Add(updateWritingResponse!.Id);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            foreach (var createdId in _createdIds)
            {
                await _httpClient.DeleteAsync($"api/writings/{createdId}");
            }
        }
    }
}
