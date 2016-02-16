using System.Threading.Tasks;
using Abp.Application.Services;
using KyleTanczos.TestKyle.Authorization.Users.Profile.Dto;

namespace KyleTanczos.TestKyle.Authorization.Users.Profile
{
    public interface IProfileAppService : IApplicationService
    {
        Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);
        
        Task ChangePassword(ChangePasswordInput input);
    }
}
