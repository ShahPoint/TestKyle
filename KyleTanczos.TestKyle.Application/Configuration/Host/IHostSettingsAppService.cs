using System.Threading.Tasks;
using Abp.Application.Services;
using KyleTanczos.TestKyle.Configuration.Host.Dto;

namespace KyleTanczos.TestKyle.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);
    }
}
