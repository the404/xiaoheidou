using System.Runtime.CompilerServices;
using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using Apworks.Specifications;
using EasyWeixin.Model;
using System;

namespace EasyWeixin.Data.Repositories
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        private readonly IEntityFrameworkRepositoryContext efContext;

        public UserProfileRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                this.efContext = context as IEntityFrameworkRepositoryContext;
        }

        //todo 怎么对数据做一个缓存呢,这应该是一个独立的部分
        //todo 有时间应该去好好参考下papapa项目中的东西
        /// <summary>
        /// 只是将查询到的用户信息添加到session中
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProfile FindUser(int userId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 只是将查询到的用户信息添加到session中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserProfile FindUser(Guid id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserProfileRepository : IBaseRepository<UserProfile>
    {
        UserProfile FindUser(Guid id);
        UserProfile FindUser(int userId);
    }
}