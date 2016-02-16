using Abp.Application.Features;
using KyleTanczos.TestKyle.Authorization.Roles;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.MultiTenancy;

namespace KyleTanczos.TestKyle.Editions
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager) 
            : base(tenantManager)
        {
        }
    }
}
