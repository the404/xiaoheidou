using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public PermissionRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IPermissionRepository : IBaseRepository<Permission>
    {
    }
}