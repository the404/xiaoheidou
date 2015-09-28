using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseImageRepository : BaseRepository<ResponseImage>, IResponseImageRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseImageRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseImageRepository : IBaseRepository<ResponseImage>
    {
    }
}