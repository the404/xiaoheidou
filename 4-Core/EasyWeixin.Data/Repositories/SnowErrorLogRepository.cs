using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SnowErrorLogRepository : BaseRepository<SnowErrorLog>, ISnowErrorLogRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SnowErrorLogRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISnowErrorLogRepository : IBaseRepository<SnowErrorLog>
    {
    }
}