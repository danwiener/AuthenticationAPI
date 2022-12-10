using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
    public class ResetToken
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		[JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
