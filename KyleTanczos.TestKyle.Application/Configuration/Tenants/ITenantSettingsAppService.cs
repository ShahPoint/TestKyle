using System.Threading.Tasks;
using Abp.Application.Services;
using KyleTanczos.TestKyle.Configuration.Tenants.Dto;

namespace KyleTanczos.TestKyle.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
