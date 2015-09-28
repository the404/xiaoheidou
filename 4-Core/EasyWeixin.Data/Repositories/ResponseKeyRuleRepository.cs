using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ResponseKeyRuleRepository : BaseRepository<ResponseKeyRule>, IResponseKeyRuleRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ResponseKeyRuleRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IResponseKeyRuleRepository : IBaseRepository<ResponseKeyRule>
    {
    }
}