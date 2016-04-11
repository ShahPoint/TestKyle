using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNet.Identity;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.MultiTenancy;
using Abp.Organizations;
using System.Collections.Generic;
using System.Linq;

namespace KyleTanczos.TestKyle
{
    /// <summary>
    /// All application services in this application is derived from this class.
    /// We can add common application service methods here.
    /// </summary>
    public abstract class TestKyleAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        public OrganizationUnitManager OrganizationUnitManager { get; set; }

        protected TestKyleAppServiceBase()
        {
            LocalizationSourceName = TestKyleConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual User GetCurrentUser()
        {
            var user = UserManager.FindById(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual Tenant GetCurrentTenant()
        {
            return TenantManager.GetById(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual async Task<OrganizationUnit> GetCurrentOrganizationUnitAsync()
        {
            User currentUser = await GetCurrentUserAsync();
            List<OrganizationUnit> units = await UserManager.GetOrganizationUnitsAsync(currentUser);
            OrganizationUnit org = units.FirstOrDefault();
            if (org == null)
            {
                throw new ApplicationException("There is no current organization!");
            }
            return org;
        }
        //protected virtual OrganizationUnit GetCurrentOrganizationUnit()
        //{
        //    return OrganizationUnitManager.
        //}
    }
}