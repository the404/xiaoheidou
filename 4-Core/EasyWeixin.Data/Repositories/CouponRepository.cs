using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public CouponRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ICouponRepository : IBaseRepository<Coupon>
    {
    }
}