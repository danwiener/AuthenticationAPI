using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class AddPlayerDTO
	{
		[JsonPropertyName("PlayerId")]
		public int PlayerId { get; set; }


		[JsonPropertyName("teamid")]
		public int TeamId { get; set; } // Fantasy team player associated with
	}
}
