using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class VoteRepository : BaseRepository<Vote>, IVoteRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public VoteRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IVoteRepository : IBaseRepository<Vote>
    {
    }
}