using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class WheelItemRepository : BaseRepository<WheelItem>, IWheelItemRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public WheelItemRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IWheelItemRepository : IBaseRepository<WheelItem>
    {
    }
}