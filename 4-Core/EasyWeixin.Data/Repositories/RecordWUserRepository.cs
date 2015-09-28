using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class RecordWUserRepository : BaseRepository<RecordWUser>, IRecordWUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public RecordWUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IRecordWUserRepository : IBaseRepository<RecordWUser>
    {
    }
}