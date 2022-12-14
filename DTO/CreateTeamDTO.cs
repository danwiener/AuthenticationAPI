using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class CreateTeamDTO
	{
		[JsonPropertyName("teamname")]
		public string TeamName { get; set; }

		[JsonPropertyName("createdondate")]
		public DateTime CreatedOnDate { get; set; }

		[JsonPropertyName("creatorid")]
		public int Creator { get; set; }

		[JsonPropertyName("leagueid")]
		public int League { get; set; }
	}
}
