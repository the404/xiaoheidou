using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseKeyRepository : BaseRepository<ResponseKey>, IResponseKeyRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseKeyRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseKeyRepository : IBaseRepository<ResponseKey>
    {
    }
}