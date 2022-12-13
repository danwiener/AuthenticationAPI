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

		[JsonPropertyName("teamname")]
		public string TeamName { get; set; }

		[JsonPropertyName("createdondate")]
		public DateTime CreatedOnDate { get; set; }

		[JsonPropertyName("creatorid")]
		public int Creator { get; set; }

		public League_Team League_Team { get; set; }

	}
}
