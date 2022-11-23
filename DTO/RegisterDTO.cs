using System.Text.Json.Serialization;

namespace Authentication.DTO
{
    public class RegisterDTO
    {

        [JsonPropertyName("user_name")]
        public string Username { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = default!;

        [JsonPropertyName("password_confirm")]
        public string PasswordConfirm { get; set; } = default!;

    }
}
