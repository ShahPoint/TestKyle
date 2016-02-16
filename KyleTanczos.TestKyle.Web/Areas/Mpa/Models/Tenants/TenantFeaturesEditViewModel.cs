using Abp.AutoMapper;
using KyleTanczos.TestKyle.MultiTenancy;
using KyleTanczos.TestKyle.MultiTenancy.Dto;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesForEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesForEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }

        public TenantFeaturesEditViewModel(Tenant tenant, GetTenantFeaturesForEditOutput output)
        {
            Tenant = tenant;
            output.MapTo(this);
        }
    }
}