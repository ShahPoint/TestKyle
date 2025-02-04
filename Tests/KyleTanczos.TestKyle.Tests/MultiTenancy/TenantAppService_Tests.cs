﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Zero.Configuration;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.MultiTenancy;
using KyleTanczos.TestKyle.MultiTenancy.Dto;
using Shouldly;
using Xunit;

namespace KyleTanczos.TestKyle.Tests.MultiTenancy
{
    public class TenantAppService_Tests : AppTestBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantAppService_Tests()
        {
            LoginAsHostAdmin();
            _tenantAppService = Resolve<ITenantAppService>();
        }

        [Fact]
        public async Task GetTenants_Test()
        {
            //Act
            var output = await _tenantAppService.GetTenants(new GetTenantsInput());

            //Assert
            output.TotalCount.ShouldBe(1);
            output.Items.Count.ShouldBe(1);
            output.Items[0].TenancyName.ShouldBe(Tenant.DefaultTenantName);
        }

        [Fact]
        public async Task Create_Update_And_Delete_Tenant_Test()
        {
            //CREATE --------------------------------

            //Act
            await _tenantAppService.CreateTenant(
                new CreateTenantInput
                {
                    TenancyName = "testTenant",
                    Name = "Tenant for test purpose",
                    AdminEmailAddress = "admin@testtenant.com",
                    AdminPassword = "123qwe",
                    IsActive = true
                });

            //Assert
            var tenant = await GetTenantOrNullAsync("testTenant");
            tenant.ShouldNotBe(null);
            tenant.Name.ShouldBe("Tenant for test purpose");
            tenant.IsActive.ShouldBe(true);

            await UsingDbContext(
                async context =>
                {
                    //Check static roles
                    var staticRoleNames = Resolve<IRoleManagementConfig>().StaticRoles.Select(role => role.RoleName).ToList();
                    foreach (var staticRoleName in staticRoleNames)
                    {
                        (await context.Roles.CountAsync(r => r.TenantId == tenant.Id && r.Name == staticRoleName)).ShouldBe(1);
                    }

                    //Check default admin user
                    (await context.Users.CountAsync(u => u.TenantId == tenant.Id && u.UserName == User.AdminUserName)).ShouldBe(1);
                });

            //GET FOR EDIT -----------------------------

            //Act
            var editDto = await _tenantAppService.GetTenantForEdit(new EntityRequestInput(tenant.Id));

            //Assert
            editDto.TenancyName.ShouldBe("testTenant");
            editDto.Name.ShouldBe("Tenant for test purpose");
            editDto.IsActive.ShouldBe(true);

            // UPDATE ----------------------------------

            editDto.Name = "edited tenant name";
            editDto.IsActive = false;
            await _tenantAppService.UpdateTenant(editDto);

            //Assert
            tenant = await GetTenantAsync("testTenant");
            tenant.Name.ShouldBe("edited tenant name");
            tenant.IsActive.ShouldBe(false);

            // DELETE ----------------------------------

            //Act
            await _tenantAppService.DeleteTenant(new EntityRequestInput((await GetTenantAsync("testTenant")).Id));

            //Assert
            (await GetTenantOrNullAsync("testTenant")).IsDeleted.ShouldBe(true);
        }
    }
}
