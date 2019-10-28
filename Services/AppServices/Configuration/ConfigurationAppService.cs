using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MyXSpace.AppServices.Configuration.Dto;
using MyXSpace.Core.Services;

namespace MyXSpace.AppServices.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            //await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), 
            //    AppSettingNames.UiTheme, input.Theme);
        }

        public ConfigurationAppService(ITenantManager tenantManager) :base(tenantManager)//: base(DependencyResolver.Current.GetService <>)
        {
        }
    }
}
