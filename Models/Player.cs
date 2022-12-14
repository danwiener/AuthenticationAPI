using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
	public class Player
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		[JsonPropertyName("PlayerId")]
		public int PlayerId { get; set; }

		[JsonPropertyName("leagueid")]
		public int LeagueId { get; set; } = default!;

		[JsonPropertyName("teamid")]
		public int? TeamId { get; set; } = default!; // Fantasy team player associated with

		[JsonPropertyName("position")]
		public string Position { get; set; } = default!;

		[JsonPropertyName("playername")]
		public string PlayerName { get; set; } = default!;

		[JsonPropertyName("team")]
		public string Team { get; set; } = default!;

	}
}
