using Microsoft.EntityFrameworkCore;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {
        public PlayerRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Players.Load();
        }
    }
}