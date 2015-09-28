using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ScratchRepository : BaseRepository<Scratch>, IScratchRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ScratchRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IScratchRepository : IBaseRepository<Scratch>
    {
    }
}