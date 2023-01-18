using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SRCContext _context;

        public IPlayerRepository Players { get; set; }
        public IGameRepository Games { get; set; }
        public IPlatformRepository Platforms { get; set; }
        public ICategoryRepository Categories { get; set; }
        public IGameCategoryRepository GameCategories { get; set; }
        public IResultRepository Results { get; set; }

        public UnitOfWork(SRCContext context)
        {
            _context       = context;
            Players        = new PlayerRepository(context);
            Games          = new GameRepository(context);
            Platforms      = new PlatformRepository(context);
            Categories     = new CategoryRepository(context);
            GameCategories = new GameCategoryRepository(context);
            Results        = new ResultRepository(context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}