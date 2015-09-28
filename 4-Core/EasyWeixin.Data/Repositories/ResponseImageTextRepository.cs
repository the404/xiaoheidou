using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseImageTextRepository : BaseRepository<ResponseImageText>, IResponseImageTextRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseImageTextRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseImageTextRepository : IBaseRepository<ResponseImageText>
    {
    }
}