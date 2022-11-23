using System.Text.Json.Serialization;

namespace Authentication.DTO
{
    public class LoginDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
