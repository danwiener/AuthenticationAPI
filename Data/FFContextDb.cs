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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Email).IsUnique(); });
            modelBuilder.Entity<ResetToken>(entity => { entity.HasIndex(e => e.Token).IsUnique(); });
        }
    }
}
