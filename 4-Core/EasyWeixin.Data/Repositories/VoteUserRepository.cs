using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class VoteUserRepository : BaseRepository<VoteUser>, IVoteUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public VoteUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IVoteUserRepository : IBaseRepository<VoteUser>
    {
    }
}