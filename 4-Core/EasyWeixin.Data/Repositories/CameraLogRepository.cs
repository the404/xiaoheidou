using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class CameraLogRepository : BaseRepository<CameraLog>, ICameraLogRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public CameraLogRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ICameraLogRepository : IBaseRepository<CameraLog>
    {
    }
}