using Abp.Application.Services;
using KyleTanczos.TestKyle.Tenants.Dashboard.Dto;

namespace KyleTanczos.TestKyle.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();
    }
}
