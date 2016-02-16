using System.Collections.Generic;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.Editions.Dto;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}