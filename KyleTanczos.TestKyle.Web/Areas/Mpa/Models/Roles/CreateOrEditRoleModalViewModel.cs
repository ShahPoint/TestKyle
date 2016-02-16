using Abp.AutoMapper;
using KyleTanczos.TestKyle.Authorization.Roles.Dto;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode
        {
            get { return Role.Id.HasValue; }
        }

        public CreateOrEditRoleModalViewModel(GetRoleForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}