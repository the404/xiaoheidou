using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ScratchUserRepository : BaseRepository<ScratchUser>, IScratchUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ScratchUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IScratchUserRepository : IBaseRepository<ScratchUser>
    {
    }
}