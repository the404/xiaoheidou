using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class GuessRepository : BaseRepository<Guess>, IGuessRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public GuessRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IGuessRepository : IBaseRepository<Guess>
    {
    }
}