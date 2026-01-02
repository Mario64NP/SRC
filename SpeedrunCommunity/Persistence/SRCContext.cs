using SpeedrunCommunity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public SRCContext()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            if (configuration["DatabaseProvider"] == "Sqlite")
            {
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();
            string provider = configuration["DatabaseProvider"] ?? "Sqlite";

            if (provider == "Sqlite")
            {
                optionsBuilder.UseSqlite(configuration.GetConnectionString("Sqlite"));
            }
            else
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCategory>().HasKey(gc => new { gc.GameID, gc.CategoryID });
            modelBuilder.Entity<Result>().HasKey(r => new { r.PlayerID, r.GameID, r.CategoryID });
            modelBuilder.Entity<Result>().HasOne(r => r.GameCategory).WithMany();
        }
    }
}
