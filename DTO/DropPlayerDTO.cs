using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class DropPlayerDTO
	{
		[JsonPropertyName("PlayerId")]
		public int PlayerId { get; set; }
	}
}
