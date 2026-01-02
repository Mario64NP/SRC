using System.Linq;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(SRCContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Platforms.Any())
            {
                context.Platforms.AddRange(
                    new Platform { Name = "PC" },
                    new Platform { Name = "PS5" },
                    new Platform { Name = "Xbox Series X" },
                    new Platform { Name = "Switch" },
                    new Platform { Name = "Retro Console" }
                );
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Any%", Description = "Finish the game as fast as possible." },
                    new Category { Name = "100%", Description = "Collect all items and finish the game." },
                    new Category { Name = "Low%", Description = "Finish the game with the minimum number of items." },
                    new Category { Name = "Glitchless", Description = "Finish the game without using major glitches." }
                );
                context.SaveChanges();
            }

            // User explicitly requested NOT to seed players.
        }
    }
}
