using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SnowUserRepository : BaseRepository<SnowUser>, ISnowUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SnowUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISnowUserRepository : IBaseRepository<SnowUser>
    {
    }
}