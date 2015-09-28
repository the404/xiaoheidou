using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class PermissionsInRolesRepository : BaseRepository<PermissionsInRoles>, IPermissionsInRolesRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public PermissionsInRolesRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IPermissionsInRolesRepository : IBaseRepository<PermissionsInRoles>
    {
    }
}