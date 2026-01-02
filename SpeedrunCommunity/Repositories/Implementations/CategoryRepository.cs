using Microsoft.EntityFrameworkCore;
using SpeedrunCommunity.Persistence;
using SpeedrunCommunity.Repositories.Interfaces;
using SpeedrunCommunity.Domain;

namespace SpeedrunCommunity.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Categories.Load();
        }
    }
}
