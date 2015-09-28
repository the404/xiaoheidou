using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class CameraPhotoRepository : BaseRepository<CameraPhoto>, ICameraPhotoRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public CameraPhotoRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ICameraPhotoRepository : IBaseRepository<CameraPhoto>
    {
    }
}