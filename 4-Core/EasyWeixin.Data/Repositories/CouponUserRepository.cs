using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class CouponUserRepository : BaseRepository<CouponUser>, ICouponUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public CouponUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ICouponUserRepository : IBaseRepository<CouponUser>
    {
    }
}