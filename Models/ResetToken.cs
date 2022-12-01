using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
    public class ResetToken
    {
        [Key()]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
