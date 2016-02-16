using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using KyleTanczos.TestKyle.MultiTenancy;

namespace KyleTanczos.TestKyle.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public string EditionDisplayName { get; set; }
    }
}