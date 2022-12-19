using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
	public class Team
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		[JsonPropertyName("TeamId")]
		public int TeamId { get; set; }

		[JsonPropertyName("teamname")]
		public string TeamName { get; set; } = default!;

		[JsonPropertyName("createdondate")]
		public DateTime CreatedOnDate { get; set; } = default!;

		[JsonPropertyName("creatorid")]
		public int Creator { get; set; } = default!;

		[JsonPropertyName("leagueid")]
		public int League { get; set; } = default!;


	}
}
