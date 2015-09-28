using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class PayCustomerRepository : BaseRepository<PayCustomer>, IPayCustomerRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public PayCustomerRepository(IRepositoryContext context)
           : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }
    }

    public interface IPayCustomerRepository : IBaseRepository<PayCustomer>
    {
    }
}