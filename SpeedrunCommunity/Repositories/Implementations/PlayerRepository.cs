using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.Repositories.Interfaces;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Repositories.Implementations
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Players.Load();
        }
    }
}
