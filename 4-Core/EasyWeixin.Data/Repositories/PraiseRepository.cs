using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class PraiseRepository : BaseRepository<Praise>, IPraiseRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public PraiseRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IPraiseRepository : IBaseRepository<Praise>
    {
    }
}