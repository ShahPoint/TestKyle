using Abp.AutoMapper;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.Authorization.Users.Profile.Dto;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Profile
{
    [AutoMapFrom(typeof (CurrentUserProfileEditDto))]
    public class MySettingsViewModel : CurrentUserProfileEditDto
    {
        public bool CanChangeUserName
        {
            get { return UserName != User.AdminUserName; }
        }

        public MySettingsViewModel(CurrentUserProfileEditDto currentUserProfileEditDto)
        {
            currentUserProfileEditDto.MapTo(this);
        }
    }
}