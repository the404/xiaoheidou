using AutoMapper;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;
using UserProfile = EasyWeixin.Model.UserProfile;

namespace EasyWeixin.Web.Mappers
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ModelToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ResponseMessage, ResponseMessageViewModel>();
            Mapper.CreateMap<ResponseMusic, ResponseMusicViewModel>();
            Mapper.CreateMap<ResponseVideo, ResponseVideoViewModel>();
            Mapper.CreateMap<ResponseImage, ResponseImageViewModel>();
            Mapper.CreateMap<ResponseImageText, ResponseImageTextViewModel>();
            Mapper.CreateMap<Button, ButtonViewModel>();
            Mapper.CreateMap<SubButton, SubButtonViewModel>();
            Mapper.CreateMap<Permission, PermissionViewModel>();
            Mapper.CreateMap<UserProfile, PersonalInfoViewModel>();
            Mapper.CreateMap<UserProfile, PersonalWeiXinViewModel>();
            Mapper.CreateMap<GuessUser, GuessNewsViewModel>();
            Mapper.CreateMap<Guess, GuessViewModel>();

            Mapper.CreateMap<ScratchUser, ScratchUserViewModel>();
            Mapper.CreateMap<Scratch, ScratchViewModel>();
            Mapper.CreateMap<ScratchItem, ScratchItemViewModel>();

            Mapper.CreateMap<WheelUser, WheelUserViewModel>();
            Mapper.CreateMap<Wheel, WheelViewModel>();
            Mapper.CreateMap<WheelItem, WheelItemViewModel>();

            Mapper.CreateMap<VoteUser, VoteUserViewModel>();
            Mapper.CreateMap<Vote, VoteViewModel>();

            Mapper.CreateMap<CouponUser, CouponUserViewModel>();
            Mapper.CreateMap<Coupon, CouponViewModel>();
            Mapper.CreateMap<CouponItem, CouponItemViewModel>();

            Mapper.CreateMap<FightUser, FightUserViewModel>();
            Mapper.CreateMap<Fight, FightViewModel>();
            Mapper.CreateMap<FightItem, FightItemViewModel>();
            Mapper.CreateMap<FightUserItem, FightUserItemViewModel>();

            Mapper.CreateMap<Snow, SnowViewModel>();

            Mapper.CreateMap<Prefer, PreferModels>();

            Mapper.CreateMap<Act, ActModels>();

            Mapper.CreateMap<PhotoWall, CameraViewModel>();

            Mapper.CreateMap<QuestionCategory, QuestionaireViewModel>();
            Mapper.CreateMap<SetQuestion, SetQuestionViewModel>();

            //微信原始数据和微信数据库数据
            Mapper.CreateMap<WeixinUserInfoResult, WeixinUser>().ForMember("WeixinUserInUsers", s => s.Ignore());
            Mapper.CreateMap<OAuthWeixinUserInfoResult, WeixinUser>().ForMember("WeixinUserInUsers", s => s.Ignore());
            Mapper.CreateMap<WeixinUser, WeixinUserViewModel>();
            Mapper.CreateMap<WeixinUserInUsers, WeixinUserInUsersViewModel>();
            Mapper.CreateMap<WeixinUserInActivity, WeixinUserInActivitiesViewModel>();
        }
    }
}