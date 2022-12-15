using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class DeleteTeamDTO
	{
		[JsonPropertyName("teamid")]
		public int TeamId { get; set; } = default!;

		[JsonPropertyName("teamname")]
		public string TeamName { get; set; } = default!;
	}
}
