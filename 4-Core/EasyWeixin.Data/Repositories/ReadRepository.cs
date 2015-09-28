using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ReadRepository : BaseRepository<Read>, IReadRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ReadRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IReadRepository : IBaseRepository<Read>
    {
    }
}