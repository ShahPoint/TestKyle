using Abp.AutoMapper;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.Authorization.Users.Dto;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Users
{
    [AutoMapFrom(typeof(GetUserPermissionsForEditOutput))]
    public class UserPermissionsEditViewModel : GetUserPermissionsForEditOutput, IPermissionsEditViewModel
    {
        public User User { get; private set; }

        public UserPermissionsEditViewModel(GetUserPermissionsForEditOutput output, User user)
        {
            User = user;
            output.MapTo(this);
        }
    }
}