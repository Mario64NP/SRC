using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.Repositories.Interfaces;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Repositories.Implementations
{
    public class PlatformRepository : Repository<Platform>, IPlatformRepository
    {
        public PlatformRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Platforms.Load();
        }
    }
}
