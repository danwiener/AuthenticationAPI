using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
	public class Team
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		[JsonPropertyName("UserId")]
		public int TeamId { get; set; }

		[JsonPropertyName("UserId")]
		public int UserId { get; set; }

		[JsonPropertyName("LeagueId")]
		public int LeagueId { get; set; }

		[JsonPropertyName("teamname")]
		public string TeamName { get; set; }

		public DateTime Created { get; set; }

		public League_Team League_Team { get; set; }
	}
}
