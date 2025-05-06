using MailBuddy.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;

namespace MailBuddy.Services
{
    public class GoogleAuthService
    {
        // Assigning the settings(such as ClientId, ClientSerect, RedirectUris) to a private field for use throughout this class.
        private readonly GoogleAuthSettings _googleAuthSettings;

        public GoogleAuthService(IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings.Value;
        }

     public async Task<string> ExchangeCodeForTokens(string code)
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

        //How do you know when to use await?
        //Use await when:
        //The method name ends with Async(most of the time).
        //The method returns a Task or Task<T>.
        //The operation involves waiting(e.g.web requests, reading files, database access).
        var response = await client.PostAsync(tokenUrl, content);
        var responseString = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString);

        // Now you have the access token and refresh token
        string accessToken = tokenResponse.AccessToken;
        string refreshToken = tokenResponse.RefreshToken;

        // Use the access token to call Gmail API
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var gmailResponse = await client.GetAsync("https://www.googleapis.com/gmail/v1/users/me/profile");
        return await gmailResponse.Content.ReadAsStringAsync();
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


