using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class UserMembershipRepository : BaseRepository<UserMembership>, IUserMembershipRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public UserMembershipRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IUserMembershipRepository : IBaseRepository<UserMembership>
    {
    }
}