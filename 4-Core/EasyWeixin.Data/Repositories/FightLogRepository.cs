using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class FightLogRepository : BaseRepository<FightLog>, IFightLogRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public FightLogRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IFightLogRepository : IBaseRepository<FightLog>
    {
    }
}