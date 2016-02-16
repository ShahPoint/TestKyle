using Abp.AutoMapper;
using KyleTanczos.TestKyle.Editions.Dto;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionForEditOutput))]
    public class CreateOrEditEditionModalViewModel : GetEditionForEditOutput, IFeatureEditViewModel
    {
        public bool IsEditMode
        {
            get { return Edition.Id.HasValue; }
        }

        public CreateOrEditEditionModalViewModel(GetEditionForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}