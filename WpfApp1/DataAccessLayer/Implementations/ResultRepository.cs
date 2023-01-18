using Microsoft.EntityFrameworkCore;
using WpfApp.Controller;
using WpfApp.DataAccessLayer.Interfaces;
using WpfApp.Domain;

namespace WpfApp.DataAccessLayer.Implementations
{
    public class ResultRepository : Repository<Result>, IResultRepository
    {
        public ResultRepository(SRCContext dbContext) : base(dbContext)
        {
            dbContext.Results.Load();
        }
    }
}