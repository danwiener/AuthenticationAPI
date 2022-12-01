using System.Text.Json.Serialization;

namespace Authentication.DTO
{
    public class ResetDTO
    {
		[JsonPropertyName("token")]
		public string Token { get; set; } = default!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = default!;

        [JsonPropertyName("password_confirm")]
        public string PasswordConfirm { get; set; } = default!;

    }
}
