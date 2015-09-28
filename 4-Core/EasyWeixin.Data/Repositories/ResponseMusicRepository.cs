using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseMusicRepository : BaseRepository<ResponseMusic>, IResponseMusicRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseMusicRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseMusicRepository : IBaseRepository<ResponseMusic>
    {
    }
}