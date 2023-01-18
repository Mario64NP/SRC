using Microsoft.EntityFrameworkCore;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class PlatformRepository : Repository<Platform>, IPlatformRepository
    {
        public PlatformRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Platforms.Load();
        }
    }
}