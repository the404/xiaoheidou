using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class FightRepository : BaseRepository<Fight>, IFightRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public FightRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IFightRepository : IBaseRepository<Fight>
    {
    }
}