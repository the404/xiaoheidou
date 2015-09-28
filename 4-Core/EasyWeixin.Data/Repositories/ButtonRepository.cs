using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class ButtonRepository : BaseRepository<Button>, IButtonRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public ButtonRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IButtonRepository : IBaseRepository<Button>
    {
    }
}