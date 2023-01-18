using Microsoft.EntityFrameworkCore;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Games.Load();
        }
    }
}