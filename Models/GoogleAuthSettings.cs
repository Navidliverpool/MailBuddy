namespace MailBuddy.Models
{
    public class GoogleAuthSettings
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public List<string>? RedirectUris { get; set; } // for multiple redirect URIs
    }
}
