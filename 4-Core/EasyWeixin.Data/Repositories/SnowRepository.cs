using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SnowRepository : BaseRepository<Snow>, ISnowRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SnowRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISnowRepository : IBaseRepository<Snow>
    {
    }
}