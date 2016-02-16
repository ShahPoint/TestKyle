using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Authorization.Users.Profile;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Profile;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : Controller
    {
        private readonly IProfileAppService _profileAppService;

        public ProfileController(IProfileAppService profileAppService)
        {
            _profileAppService = profileAppService;
        }

        public async Task<PartialViewResult> MySettingsModal()
        {
            var output = await _profileAppService.GetCurrentUserProfileForEdit();
            var viewModel = new MySettingsViewModel(output);

            return PartialView("_MySettingsModal", viewModel);
        }

        public PartialViewResult ChangePictureModal()
        {
            return PartialView("_ChangePictureModal");
        }

        public PartialViewResult ChangePasswordModal()
        {
            return PartialView("_ChangePasswordModal");
        }
    }
}