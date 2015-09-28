using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class CouponItemRepository : BaseRepository<CouponItem>, ICouponItemRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public CouponItemRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ICouponItemRepository : IBaseRepository<CouponItem>
    {
    }
}