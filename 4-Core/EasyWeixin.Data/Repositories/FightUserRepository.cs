using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class FightUserRepository : BaseRepository<FightUser>, IFightUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public FightUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IFightUserRepository : IBaseRepository<FightUser>
    {
    }
}