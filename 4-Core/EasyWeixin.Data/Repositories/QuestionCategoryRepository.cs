using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class QuestionCategoryRepository : BaseRepository<QuestionCategory>, IQuestionCategoryRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public QuestionCategoryRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IQuestionCategoryRepository : IBaseRepository<QuestionCategory>
    {
    }
}