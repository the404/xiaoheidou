using AutoMapper;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;

namespace EasyWeixin.Web.Mappers
{
    public class ViewModelToModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToModelMapping"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ResponseMessageViewModel, ResponseMessage>();
            Mapper.CreateMap<ResponseMusicViewModel, ResponseMusic>();
            Mapper.CreateMap<ResponseVideoViewModel, ResponseVideo>();
            Mapper.CreateMap<ResponseImageViewModel, ResponseImage>();
            Mapper.CreateMap<ResponseImageTextViewModel, ResponseImageText>();
            Mapper.CreateMap<ButtonViewModel, Button>();
            Mapper.CreateMap<SubButtonViewModel, SubButton>();
            Mapper.CreateMap<PermissionViewModel, Permission>();
            Mapper.CreateMap<PersonalInfoViewModel, EasyWeixin.Model.UserProfile>();
            Mapper.CreateMap<PersonalWeiXinViewModel, EasyWeixin.Model.UserProfile>();

            Mapper.CreateMap<GuessNewsViewModel, GuessUser>();
            Mapper.CreateMap<GuessViewModel, Guess>();

            Mapper.CreateMap<ScratchUserViewModel, ScratchUser>();
            Mapper.CreateMap<ScratchViewModel, Scratch>();
            Mapper.CreateMap<ScratchItemViewModel, ScratchItem>();

            Mapper.CreateMap<WheelUserViewModel, WheelUser>();
            Mapper.CreateMap<WheelViewModel, Wheel>();
            Mapper.CreateMap<WheelItemViewModel, WheelItem>();

            Mapper.CreateMap<VoteUserViewModel, VoteUser>();
            Mapper.CreateMap<VoteViewModel, Vote>();

            Mapper.CreateMap<CouponUserViewModel, CouponUser>();
            Mapper.CreateMap<CouponViewModel, Coupon>();
            Mapper.CreateMap<CouponItemViewModel, CouponItem>();

            Mapper.CreateMap<FightUserViewModel, FightUser>();
            Mapper.CreateMap<FightViewModel, Fight>();
            Mapper.CreateMap<FightItemViewModel, FightItem>();
            Mapper.CreateMap<FightUserItemViewModel, FightUserItem>();

            Mapper.CreateMap<SnowViewModel, Snow>();

            //by tianxiu
            Mapper.CreateMap<PreferModels, Prefer>();
            Mapper.CreateMap<ActModels, Act>();

            Mapper.CreateMap<CameraViewModel, PhotoWall>();

            Mapper.CreateMap<QuestionaireViewModel, QuestionCategory>();
            Mapper.CreateMap<SetQuestionViewModel, SetQuestion>();

            //微信用户
            Mapper.CreateMap<WeixinUser, WeixinUserInfoResult>();
            Mapper.CreateMap<WeixinUser, OAuthWeixinUserInfoResult>();
            Mapper.CreateMap<WeixinUserViewModel, WeixinUser>();
            Mapper.CreateMap<WeixinUserInUsersViewModel, WeixinUserInUsers>();
            Mapper.CreateMap<WeixinUserInActivitiesViewModel, WeixinUserInActivity>();
        }
    }
}