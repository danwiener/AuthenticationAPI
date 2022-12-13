using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
	public class League_Team
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		public int Id { get; set; }
		public int LeagueId { get; set; }
		public League League { get; set; }

		public int TeamId { get; set; }
		public Team Team { get; set; }
	}
}
