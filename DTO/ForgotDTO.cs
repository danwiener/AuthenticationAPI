using System.Text.Json.Serialization;

namespace Authentication.DTO
{
    public class ForgotDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;

    }
}
