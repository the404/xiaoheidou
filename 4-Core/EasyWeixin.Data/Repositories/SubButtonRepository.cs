using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SubButtonRepository : BaseRepository<SubButton>, ISubButtonRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SubButtonRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISubButtonRepository : IBaseRepository<SubButton>
    {
    }
}