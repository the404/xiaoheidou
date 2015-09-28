using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class QItemAnswerRepository : BaseRepository<QItemAnswer>, IQItemAnswerRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public QItemAnswerRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IQItemAnswerRepository : IBaseRepository<QItemAnswer>
    {
    }
}