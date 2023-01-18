using Microsoft.EntityFrameworkCore;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class GameCategoryRepository : Repository<GameCategory>, IGameCategoryRepository
    {
        public GameCategoryRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.GameCategories.Load();
        }
    }
}