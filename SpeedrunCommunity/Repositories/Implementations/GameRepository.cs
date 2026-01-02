using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.Repositories.Interfaces;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Repositories.Implementations
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Games.Load();
        }
    }
}
