using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class PreferRepository : BaseRepository<Prefer>, IPreferRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public PreferRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IPreferRepository : IBaseRepository<Prefer>
    {
    }
}