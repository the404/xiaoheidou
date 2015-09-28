using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class WheelUserRepository : BaseRepository<WheelUser>, IWheelUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public WheelUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IWheelUserRepository : IBaseRepository<WheelUser>
    {
    }
}