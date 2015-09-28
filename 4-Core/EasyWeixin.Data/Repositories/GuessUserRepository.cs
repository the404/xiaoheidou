using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class GuessUserRepository : BaseRepository<GuessUser>, IGuessUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public GuessUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IGuessUserRepository : IBaseRepository<GuessUser>
    {
    }
}