using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SnowLogRepository : BaseRepository<SnowLog>, ISnowLogRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SnowLogRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISnowLogRepository : IBaseRepository<SnowLog>
    {
    }
}