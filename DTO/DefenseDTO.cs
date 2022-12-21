using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class DefenseDTO
	{
		[JsonPropertyName("LeagueId")]
		public int LeagueId { get; set; }

		[JsonPropertyName("defensecount")]
		public int DCount { get; set; }
	}
}
