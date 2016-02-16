using System.Linq;
using Abp;
using Abp.Authorization;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Tenants.Dashboard.Dto;

namespace KyleTanczos.TestKyle.Tenants.Dashboard
{
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : TestKyleAppServiceBase, ITenantDashboardAppService
    {
        public GetMemberActivityOutput GetMemberActivity()
        {
            //Generating some random data. We could get numbers from database...
            return new GetMemberActivityOutput
                   {
                       TotalMembers = Enumerable.Range(0, 13).Select(i => RandomHelper.GetRandom(15, 40)).ToList(),
                       NewMembers = Enumerable.Range(0, 13).Select(i => RandomHelper.GetRandom(3, 15)).ToList()
                   };
        }
    }
}