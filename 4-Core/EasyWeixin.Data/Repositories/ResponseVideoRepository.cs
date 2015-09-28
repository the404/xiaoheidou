using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseVideoRepository : BaseRepository<ResponseVideo>, IResponseVideoRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseVideoRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseVideoRepository : IBaseRepository<ResponseVideo>
    {
    }
}