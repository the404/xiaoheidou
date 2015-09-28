using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using Apworks.Specifications;
using EasyWeixin.Model;
using System;

namespace EasyWeixin.Data.Repositories
{
    public class WeixinUserInUsersRepository : BaseRepository<WeixinUserInUsers>,
        IWeixinUserInUsersRepository
    {
        private IEntityFrameworkRepositoryContext _efContext;

        public WeixinUserInUsersRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                _efContext = context as IEntityFrameworkRepositoryContext;
        }

        public WeixinUserInUsers AddOrUpdateWeixinUserInUser(WeixinUserInUsers weixinUserInUsers)
        {
            if (Exists(weixinUserInUsers))
            {
                Update(weixinUserInUsers);
                return weixinUserInUsers;
            }
            Add(weixinUserInUsers);
            return weixinUserInUsers;
        }

        public bool Exists(WeixinUserInUsers weixinUserInUsers)
        {
            if (Exists(Specification<WeixinUserInUsers>.Eval(
                s => s.UserId == weixinUserInUsers.UserId
                && s.WeixinUserId == weixinUserInUsers.WeixinUserId)))
            {
                return true;
            }
            return false;
        }

        public void AddWeixinUserInUsers(UserProfile userProfile, WeixinUser weixinUser)
        {
            var weixinUserInUsers = new WeixinUserInUsers
            {
                UserId = userProfile.UserId,
                WeixinUserId = weixinUser.WeixinUserId,
                AddDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
            if (!this.Exists(weixinUserInUsers))
            {
                this.Add(weixinUserInUsers);
                this.Context.Commit();
            }
        }
    }

    public interface IWeixinUserInUsersRepository : IBaseRepository<WeixinUserInUsers>
    {
        WeixinUserInUsers AddOrUpdateWeixinUserInUser(WeixinUserInUsers weixinUserInUsers);

        bool Exists(WeixinUserInUsers weixinUserInUsers);

        void AddWeixinUserInUsers(UserProfile userProfile, WeixinUser weixinUser);
    }
}