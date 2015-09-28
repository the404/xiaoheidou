using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Apworks.Application;
using Apworks.Config.Fluent;
using Apworks.Repositories;
using Apworks.Repositories.EntityFramework;
using EasyWeixin.Data;
using EasyWeixin.Model;
using EasyWeixin.Data.Repositories;

namespace EasyWeixin.Web.Tests
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

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
        container.RegisterInstance<EasyWeixinDbContext>(new EasyWeixinDbContext(), new PerResolveLifetimeManager())
                      .RegisterType<IRepositoryContext, EntityFrameworkRepositoryContext>(new HierarchicalLifetimeManager(),
                          new InjectionConstructor(new ResolvedParameter<EasyWeixinDbContext>()))
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

                     ;
    }
  }
}