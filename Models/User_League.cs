using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Models
{
	public class User_League
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; }

		public int LeagueId { get; set; }
		public League League { get; set; }
	}
}
