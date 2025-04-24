using MailBuddy.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MailBuddy.Services
{
    public class GoogleAuthService
    {
        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleAuthService(IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings.Value;
        }

     public async Task ExchangeCodeForTokens(string code)
    {
        var client = new HttpClient();
        var tokenUrl = "https://oauth2.googleapis.com/token";

        var values = new Dictionary<string, string>
    {
        { "code", code },
        { "client_id", _googleAuthSettings.ClientId },
        { "client_secret", _googleAuthSettings.ClientSecret },
        { "redirect_uri", _googleAuthSettings.RedirectUris[0] },
        { "grant_type", "authorization_code" }
    };

        var content = new FormUrlEncodedContent(values);

        var response = await client.PostAsync(tokenUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString);

        // Now you have the access token and refresh token
        string accessToken = tokenResponse.AccessToken;
        string refreshToken = tokenResponse.RefreshToken;

        // Use the access token to make authorized API calls
    }
}
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

}


