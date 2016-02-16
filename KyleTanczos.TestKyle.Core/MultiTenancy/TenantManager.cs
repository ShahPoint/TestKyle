using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using KyleTanczos.TestKyle.Authorization.Roles;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.Editions;

namespace KyleTanczos.TestKyle.MultiTenancy
{
    /// <summary>
    /// Tenant manager.
    /// </summary>
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager) : 
            base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager
            )
        {
        }
    }
}
