using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using Apworks.Specifications;
using EasyWeixin.Model;

namespace EasyWeixin.Data.Repositories
{
    public class WeixinUserInActivityRepository : BaseRepository<WeixinUserInActivity>
        , IWeixinUserInActivityRepository
    {
        private IEntityFrameworkRepositoryContext _efContext;

        public WeixinUserInActivityRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEntityFrameworkRepositoryContext)
                _efContext = context as IEntityFrameworkRepositoryContext;
        }

        public WeixinUserInActivity AddOrUpdateWeixinUserInActivity(WeixinUserInActivity weixinUserInActivity)
        {
            if (Exists(weixinUserInActivity))
            {
                Update(weixinUserInActivity);
                return weixinUserInActivity;
            }

            Add(weixinUserInActivity);
            return weixinUserInActivity;
        }

        public bool Exists(WeixinUserInActivity weixinUserInActivity)
        {
            if (Exists(Specification<WeixinUserInActivity>.Eval(
                s => s.ActType == ActType.Scratch
                && s.ActId == weixinUserInActivity.ActId
                && s.WeixinUserId == weixinUserInActivity.WeixinUserId)))
            {
                return true;
            }
            return false;
        }
    }

    public interface IWeixinUserInActivityRepository : IBaseRepository<WeixinUserInActivity>
    {
        WeixinUserInActivity AddOrUpdateWeixinUserInActivity(WeixinUserInActivity weixinUserInActivity);

        bool Exists(WeixinUserInActivity weixinUserInActivity);
    }
}