using Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data
{
    public class FFContextDb : DbContext
    {
        public FFContextDb(DbContextOptions<FFContextDb> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserToken> UserTokens { get; set; } = default!;
        public DbSet<ResetToken> ResetTokens { get; set; } = default!;
		public DbSet<League> Leagues { get; set; } = default!;
		public DbSet<User_League> UserLeagues { get; set; } = default!;
		public DbSet<LeagueRules> LeagueRules { get; set; } = default!;
		public DbSet<League_Team> LeagueTeams { get; set; } = default!;
		public DbSet<Team> Teams { get; set; } = default!;
		public DbSet<Player> Players { get; set; } = default!;



		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });
            modelBuilder.Entity<ResetToken>(entity => { entity.HasIndex(e => e.Token).IsUnique(); });
            modelBuilder.Entity<League>(entity => { entity.HasIndex(e => e.LeagueName).IsUnique(); });
			modelBuilder.Entity<Team>(entity => { entity.HasIndex(t => t.TeamName).IsUnique(); });

			modelBuilder.Entity<User_League>()
                .HasOne(u => u.User)
                .WithMany(ul => ul.User_Leagues)
                .HasForeignKey(ui => ui.UserId);

            modelBuilder.Entity<User_League>()
                .HasOne(l => l.League)
                .WithMany(ul => ul.User_Leagues)
                .HasForeignKey(li => li.LeagueId);

            modelBuilder.Entity<League_Team>()
                .HasOne(l => l.League)
                .WithMany(lt => lt.League_Teams)
                .HasForeignKey(li => li.LeagueId);

            modelBuilder.Entity<League_Team>()
                .HasOne(t => t.Team)
                .WithOne(lt => lt.League_Team);

            modelBuilder.Entity<League_Team>()
                .HasOne(u => u.User)
                .WithMany(lt => lt.League_Teams)
                .HasForeignKey(ui => ui.UserId);
		}
    }
}
