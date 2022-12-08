using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class DeleteLeagueDTO
	{
		[JsonPropertyName("leagueid")]
		public int LeagueId { get; set; } = default!;

		[JsonPropertyName("leaguename")]
		public string LeagueName { get; set; } = default!;
	}
}
