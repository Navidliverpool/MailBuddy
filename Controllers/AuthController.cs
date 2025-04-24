using MailBuddy.Models;
using MailBuddy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Web;
using MailBuddy.Services;

namespace MailBuddy.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly GoogleAuthService _googleAuthService;

        public AuthController(IOptions<GoogleAuthSettings> googleAuthSettings,
            GoogleAuthService googleAuthService)
        {
            _googleAuthSettings = googleAuthSettings.Value;
            _googleAuthService = googleAuthService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["client_id"] = _googleAuthSettings.ClientId;
            queryParams["redirect_uri"] = _googleAuthSettings.RedirectUris[0]; // use HTTP or HTTPS version here
            queryParams["response_type"] = "code";
            queryParams["scope"] = "https://www.googleapis.com/auth/gmail.readonly email profile";
            queryParams["access_type"] = "offline";
            queryParams["prompt"] = "consent";

            var googleOAuthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?{queryParams}";

            return Redirect(googleOAuthUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            // TODO: Exchange the code for tokens and fetch Gmail messages
            await _googleAuthService.ExchangeCodeForTokens(code);
            return Ok(new { message = "Callback received!", code });
        }
    }
}
