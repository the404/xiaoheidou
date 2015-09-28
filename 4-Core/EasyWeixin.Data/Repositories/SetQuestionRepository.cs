using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class SetQuestionRepository : BaseRepository<SetQuestion>, ISetQuestionRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public SetQuestionRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface ISetQuestionRepository : IBaseRepository<SetQuestion>
    {
    }
}