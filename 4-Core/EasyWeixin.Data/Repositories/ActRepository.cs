using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ActRepository : BaseRepository<Act>, IActRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ActRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IActRepository : IBaseRepository<Act>
    {
    }
}