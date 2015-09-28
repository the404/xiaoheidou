using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class WheelRepository : BaseRepository<Wheel>, IWheelRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public WheelRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IWheelRepository : IBaseRepository<Wheel>
    {
    }
}