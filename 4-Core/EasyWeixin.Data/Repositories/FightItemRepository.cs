using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class FightItemRepository : BaseRepository<FightItem>, IFightItemRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public FightItemRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IFightItemRepository : IBaseRepository<FightItem>
    {
    }
}