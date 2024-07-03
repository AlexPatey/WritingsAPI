using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;
using Writings.Api.Sdk;
using Writings.Api.Sdk.Consumer;
using Writings.Contracts.Requests.V1;

//Example code for how you might consume the Writings API

var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IWritingsApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (m, t) => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync()
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("base address here"));

var provider = services.BuildServiceProvider();

var writingsApi = provider.GetRequiredService<IWritingsApi>();

var writing = await writingsApi.GetWritingAsync(new Guid("id here"));

Console.WriteLine(JsonSerializer.Serialize(writing));

var request = new GetAllWritingsRequest
{
    Title = null,
    Type = null,
    YearOfCompletion = null,
    SortBy = null,
    TagId = null,
    Page = 1,
    PageSize = 5
};

var allWritings = await writingsApi.GetAllWritingsAsync(request);

allWritings.Items.ToList().ForEach(item =>
    Console.WriteLine(JsonSerializer.Serialize(item)));

Console.ReadLine();