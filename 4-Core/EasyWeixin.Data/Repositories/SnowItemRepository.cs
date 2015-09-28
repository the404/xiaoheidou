using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SnowItemRepository : BaseRepository<SnowItem>, ISnowItemRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SnowItemRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISnowItemRepository : IBaseRepository<SnowItem>
    {
    }
}