using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class FightUserItemRepository : BaseRepository<FightUserItem>, IFightUserItemRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public FightUserItemRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IFightUserItemRepository : IBaseRepository<FightUserItem>
    {
    }
}