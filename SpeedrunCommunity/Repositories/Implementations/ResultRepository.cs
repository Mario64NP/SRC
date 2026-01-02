using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.Repositories.Interfaces;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Repositories.Implementations
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        public ResultRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Results.Load();
        }
    }
}
