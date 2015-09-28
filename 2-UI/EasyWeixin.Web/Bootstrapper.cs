using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Data;
using EasyWeixin.Data.Repositories;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Unity.Mvc4;

namespace EasyWeixin.Web
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance(new EasyWeixinDbContext(), new PerResolveLifetimeManager())
                .RegisterType<IRepositoryContext, EntityFrameworkRepositoryContext>(
                    new HierarchicalLifetimeManager(),
                    new InjectionConstructor(new ResolvedParameter<EasyWeixinDbContext>())
                )
                .RegisterType(typeof(IRepository<>), typeof(EntityFrameworkRepository<>))
                .RegisterType(typeof(IPermissionRepository), typeof(PermissionRepository))
                .RegisterType(typeof(IPermissionsInRolesRepository), typeof(PermissionsInRolesRepository))
                .RegisterType(typeof(IRoleRepository), typeof(RoleRepository))
                .RegisterType(typeof(IUserMembershipRepository), typeof(UserMembershipRepository))
                .RegisterType(typeof(IUserProfileRepository), typeof(UserProfileRepository))
                .RegisterType(typeof(IButtonRepository), typeof(ButtonRepository))
                .RegisterType(typeof(ISubButtonRepository), typeof(SubButtonRepository))
                .RegisterType(typeof(IResponseImageRepository), typeof(ResponseImageRepository))
                .RegisterType(typeof(IResponseMusicRepository), typeof(ResponseMusicRepository))
                .RegisterType(typeof(IResponseVideoRepository), typeof(ResponseVideoRepository))
                .RegisterType(typeof(IResponseImageTextRepository), typeof(ResponseImageTextRepository))
                .RegisterType(typeof(IResponseMessageRepository), typeof(ResponseMessageRepository))
                .RegisterType(typeof(IResponseKeyRepository), typeof(ResponseKeyRepository))
                .RegisterType(typeof(IResponseKeyRuleRepository), typeof(ResponseKeyRuleRepository))
                //������ħ���²²�
                .RegisterType(typeof(IGuessUserRepository), typeof(GuessUserRepository))
                .RegisterType(typeof(IGuessRepository), typeof(GuessRepository))
                //������ �ι���
                .RegisterType(typeof(IScratchUserRepository), typeof(ScratchUserRepository))
                .RegisterType(typeof(IScratchRepository), typeof(ScratchRepository))
                .RegisterType(typeof(IScratchItemRepository), typeof(ScratchItemRepository))
                //������ ��ת��
                .RegisterType(typeof(IWheelLogRepository), typeof(WheelLogRepository))
                .RegisterType(typeof(IWheelRepository), typeof(WheelRepository))
                .RegisterType(typeof(IWheelItemRepository), typeof(WheelItemRepository))
                .RegisterType(typeof(IWheelUserRepository), typeof(WheelUserRepository))
                //�������Ż݄�
                .RegisterType(typeof(ICouponLogRepository), typeof(CouponLogRepository))
                .RegisterType(typeof(ICouponRepository), typeof(CouponRepository))
                .RegisterType(typeof(ICouponUserRepository), typeof(CouponUserRepository))
                .RegisterType(typeof(ICouponItemRepository), typeof(CouponItemRepository))
                //������ͶƱ
                .RegisterType(typeof(IVoteRepository), typeof(VoteRepository))
                .RegisterType(typeof(IVoteUserRepository), typeof(VoteUserRepository))
                //������һս����
                .RegisterType(typeof(IFightLogRepository), typeof(FightLogRepository))
                .RegisterType(typeof(IFightRepository), typeof(FightRepository))
                .RegisterType(typeof(IFightItemRepository), typeof(FightItemRepository))
                .RegisterType(typeof(IFightUserRepository), typeof(FightUserRepository))
                .RegisterType(typeof(IFightUserItemRepository), typeof(FightUserItemRepository))

                //������ �ι���
                .RegisterType(typeof(ISnowUserRepository), typeof(SnowUserRepository))
                .RegisterType(typeof(ISnowRepository), typeof(SnowRepository))
                .RegisterType(typeof(ISnowItemRepository), typeof(SnowItemRepository))
                .RegisterType(typeof(ISnowLogRepository), typeof(SnowLogRepository))
                .RegisterType(typeof(ISnowErrorLogRepository), typeof(SnowErrorLogRepository))

                //������ ��Ƭǽ
                .RegisterType(typeof(ICameraPhotoRepository), typeof(CameraPhotoRepository))
                .RegisterType(typeof(IPhotoWallRepository), typeof(PhotoWallRepository))
                .RegisterType(typeof(ICameraLogRepository), typeof(CameraLogRepository))

                //�Żݺͻ
                .RegisterType(typeof(IPreferRepository), typeof(PreferRepository))
                .RegisterType(typeof(IActRepository), typeof(ActRepository))

                //�ʾ����
                .RegisterType(typeof(ISetQuestionRepository), typeof(SetQuestionRepository))
                .RegisterType(typeof(IQItemAnswerRepository), typeof(QItemAnswerRepository))
                .RegisterType(typeof(IQuestionCategoryRepository), typeof(QuestionCategoryRepository))

                //���޺��Ķ�
                .RegisterType(typeof(IReadRepository), typeof(ReadRepository))
                .RegisterType(typeof(IPraiseRepository), typeof(PraiseRepository))

                //���ϻ��ȳ�֧���ɹ��û�
                .RegisterType(typeof(IPayCustomerRepository), typeof(PayCustomerRepository))
                //΢���û�
                .RegisterType<IWeixinUserRepository, WeixinUserRepository>()
                .RegisterType<IWeixinUserInUsersRepository, WeixinUserInUsersRepository>()
                .RegisterType<IWeixinUserInActivityRepository, WeixinUserInActivityRepository>()
                //��ʱ��ά��
                .RegisterType<IQrCodeRepository, QrCodeRepository>()
                ;
        }
    }
}