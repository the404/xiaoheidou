using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseMessageRepository : BaseRepository<ResponseMessage>, IResponseMessageRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseMessageRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseMessageRepository : IBaseRepository<ResponseMessage>
    {
    }
}