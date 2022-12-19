using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class CreateTeamDTO
	{
		[JsonPropertyName("team_name")]
		public string TeamName { get; set; }

		[JsonPropertyName("creator_id")]
		public int Creator { get; set; }

		[JsonPropertyName("league_id")]
		public int League { get; set; }
	}
}
