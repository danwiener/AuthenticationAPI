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



		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });
            modelBuilder.Entity<ResetToken>(entity => { entity.HasIndex(e => e.Token).IsUnique(); });
            modelBuilder.Entity<League>(entity => { entity.HasIndex(e => e.LeagueName).IsUnique(); });

            modelBuilder.Entity<User_League>()
                .HasOne(u => u.User)
                .WithMany(ul => ul.User_Leagues)
                .HasForeignKey(ui => ui.UserId);

            modelBuilder.Entity<User_League>()
                .HasOne(l => l.League)
                .WithMany(ul => ul.User_Leagues)
                .HasForeignKey(li => li.LeagueId);
		}
    }
}
