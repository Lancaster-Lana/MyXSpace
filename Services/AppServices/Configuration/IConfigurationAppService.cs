using System.Threading.Tasks;
using MyXSpace.AppServices.Configuration.Dto;

namespace MyXSpace.AppServices.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}