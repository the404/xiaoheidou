using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using Apworks.Specifications;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Model;
using System;
using System.Linq;

namespace EasyWeixin.Data.Repositories
{
    public class WeixinUserRepository : BaseRepository<WeixinUser>,
        IWeixinUserRepository
    {
        private readonly IEntityFrameworkRepositoryContext _efContext;

        public WeixinUserRepository(IRepositoryContext context)
            : base(context)
        {
            if (Context is IEntityFrameworkRepositoryContext)
                _efContext = context as IEntityFrameworkRepositoryContext;
        }

        public bool Exists(WeixinUser weixinUser)
        {
            if (Exists(Specification<WeixinUser>.Eval(s => s.Openid == weixinUser.Openid)))
            {
                return true;
            }
            return false;
        }

        public void AddWeixinUser(OAuthWeixinUserInfoResult oAuthWeixinUser, WeixinUser weixinUser)
        {
            var user = this.FindAll().SingleOrDefault(s => s.Openid == oAuthWeixinUser.openid);
            if (user != null)
            {
                //更新用户信息
                user.City = oAuthWeixinUser.city;
                user.Country = oAuthWeixinUser.country;
                user.Headimgurl = oAuthWeixinUser.headimgurl;
                user.Nickname = oAuthWeixinUser.nickname;
                user.Province = oAuthWeixinUser.province;
                user.Sex = oAuthWeixinUser.sex;
                user.UpdateDate = DateTime.Now;

                this.Update(user);
                this.Context.Commit();
                weixinUser.WeixinUserId = user.WeixinUserId;
            }
            else
            {
                weixinUser.AddDate = DateTime.Now;
                weixinUser.UpdateDate = DateTime.Now;
                this.Add(weixinUser);
                this.Context.Commit();
            }
        }
    }

    public interface IWeixinUserRepository : IBaseRepository<WeixinUser>
    {
        bool Exists(WeixinUser weixinUser);

        void AddWeixinUser(OAuthWeixinUserInfoResult oAuthWeixinUser, WeixinUser weixinUser);
    }
}