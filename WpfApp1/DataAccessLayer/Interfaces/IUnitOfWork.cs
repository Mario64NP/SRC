using System;

namespace WpfApp.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository Players { get; }
        IGameRepository Games { get; }
        IPlatformRepository Platforms { get; }
        ICategoryRepository Categories { get; }
        IGameCategoryRepository GameCategories { get; }
        IResultRepository Results { get; }

        int Complete();
    }
}