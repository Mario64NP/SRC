using SpeedrunCommunity.Domain;
using Microsoft.EntityFrameworkCore;

namespace SpeedrunCommunity.Persistence
{
    public class SRCContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Platform> Platforms { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<GameCategory> GameCategories { get; set; } = null!;
        public DbSet<Result> Results { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Napredne;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCategory>().HasKey(gc => new { gc.GameID, gc.CategoryID });
            modelBuilder.Entity<Result>().HasKey(r => new { r.PlayerID, r.GameID, r.CategoryID });
            modelBuilder.Entity<Result>().HasOne(r => r.GameCategory).WithMany();
        }
    }
}
