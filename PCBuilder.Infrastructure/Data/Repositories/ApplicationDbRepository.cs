using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Infrastructure.Common;

namespace PCBuilder.Infrastructure.Data.Repositories
{
    public class ApplicationDbRepository : Repository, IApplicationDbRepository
    {
        public ApplicationDbRepository(ApplicationDbContext context)
        {
            this.Context = context;
        }
    }
}
