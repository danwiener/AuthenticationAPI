using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.DTO
{
	public class CreateLeagueDTO
	{
		[JsonPropertyName("leaguename")]
		public string LeagueName { get; set; } = default!;

		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = default!;

		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!;

	}
}
