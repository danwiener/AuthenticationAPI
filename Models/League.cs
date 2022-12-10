using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Authentication.Data;
using System.Text.Json.Serialization;

namespace Authentication.Models
{
    
    public class League
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		[JsonPropertyName("LeagueId")]
		public int LeagueId { get; set; }

		[JsonPropertyName("leaguename")]
		public string LeagueName { get; set; } = default!;

		[JsonPropertyName("maxteams")]
		public int MaxTeams { get; set; } = default!;

		[JsonPropertyName("creator")]
		public int Creator { get; set; } = default!; // user id goes here

		public List<User_League> User_Leagues { get; set; }


		//public List<Team> Teams { get; set; } = default!;
	}
}
