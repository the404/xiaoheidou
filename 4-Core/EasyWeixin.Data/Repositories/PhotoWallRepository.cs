using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class PhotoWallRepository : BaseRepository<PhotoWall>, IPhotoWallRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public PhotoWallRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IPhotoWallRepository : IBaseRepository<PhotoWall>
    {
    }
}