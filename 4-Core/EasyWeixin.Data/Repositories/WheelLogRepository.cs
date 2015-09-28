using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class WheelLogRepository : BaseRepository<WheelLog>, IWheelLogRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public WheelLogRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IWheelLogRepository : IBaseRepository<WheelLog>
    {
    }
}