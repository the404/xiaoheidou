using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Core.MvcPager;
using System.Collections.Generic;

namespace EasyWeixin.Data.Repositories
{
    public class BaseRepository<T> : EntityFrameworkRepository<T>, IBaseRepository<T> where T : class, Apworks.IAggregateRoot
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public BaseRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }

        public virtual PagedList<T> GetListByPages(IList<T> items, int pageIndex, int pageSize)
        {
            return new PagedList<T>(items, pageIndex, pageSize);
        }

        public virtual PagedList<T> GetListByPages(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            return new PagedList<T>(items, pageIndex, pageSize, totalItemCount);
        }
    }

    public interface IBaseRepository<T> : IRepository<T> where T : class, Apworks.IAggregateRoot
    {
        PagedList<T> GetListByPages(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount);

        PagedList<T> GetListByPages(IList<T> items, int pageIndex, int pageSize);
    }
}