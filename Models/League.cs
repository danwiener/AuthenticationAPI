using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Authentication.Data;

namespace Authentication.Models
{
    public class League
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int LeagueId { get; set; }
        public string LeagueName { get; set; } = default!;
        public int MaxTeams { get; set; } = default!;
        public User Creator { get; set; } = default!;
        //public List<User> Users { get; set; } = default!;

        //public List<Team> Teams { get; set; } = default!;
    }
}
