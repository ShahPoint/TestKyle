using System.Collections.Generic;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.Editions.Dto;

namespace KyleTanczos.TestKyle.MultiTenancy.Dto
{
    public class GetTenantFeaturesForEditOutput : IOutputDto
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}