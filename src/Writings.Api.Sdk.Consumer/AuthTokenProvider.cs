using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using Writings.Contracts.Requests;

namespace Writings.Api.Sdk.Consumer
{
    public class AuthTokenProvider(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private string _cachedToken = string.Empty;
        private static readonly SemaphoreSlim Lock = new(1, 1);

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(_cachedToken))
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
                var expiryTimeText = jwt.Claims.Single(c => c.Type == "exp").Value;
                var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
                if (expiryDateTime > DateTime.UtcNow)
                {
                    return _cachedToken;
                }
            }

            await Lock.WaitAsync();

            var response = await _httpClient.PostAsJsonAsync("token url here", new TokenGenerationRequest
            {
                UserId = Guid.NewGuid(),
                Email = "test@email.com",
                CustomClaims = new Dictionary<string, object>
                {
                    { "admin", true },
                    { "trusted_member", true }
                }
            });

            var newToken = await response.Content.ReadAsStringAsync();

            _cachedToken = newToken;

            Lock.Release();

            return _cachedToken;
        }

        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
