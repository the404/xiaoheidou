using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class CouponLogRepository : BaseRepository<CouponLog>, ICouponLogRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public CouponLogRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ICouponLogRepository : IBaseRepository<CouponLog>
    {
    }
}