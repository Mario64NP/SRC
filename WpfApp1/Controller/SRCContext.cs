using WpfApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace WpfApp.Controller
{
    public class SRCContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GameCategory> GameCategories { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Napredne;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCategory>().HasKey(gc => new { gc.GameID, gc.CategoryID });
            modelBuilder.Entity<Result>().HasKey(r => new { r.PlayerID, r.GameID, r.CategoryID });
        }
    }
}