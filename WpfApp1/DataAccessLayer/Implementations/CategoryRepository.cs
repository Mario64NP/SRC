using Microsoft.EntityFrameworkCore;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Categories.Load();
        }
    }
}