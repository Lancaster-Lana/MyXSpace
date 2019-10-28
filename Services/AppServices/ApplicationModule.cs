using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MyXSpace.AppServices.Authorization;

namespace MyXSpace.AppServices
{
    [DependsOn(
        //typeof(AppServicesCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class AppServicesApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Configuration.Authorization.Providers.Add<AppServicesAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppServicesApplicationModule).GetAssembly());
        }
    }
}