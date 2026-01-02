using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.Repositories.Interfaces;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Repositories.Implementations
{
    public class GameCategoryRepository : Repository<GameCategory>, IGameCategoryRepository
    {
        public GameCategoryRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.GameCategories.Load();
        }
    }
}
