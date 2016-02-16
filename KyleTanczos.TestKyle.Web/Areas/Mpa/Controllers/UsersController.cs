using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Users;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : TestKyleControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly UserManager _userManager;

        public UsersController(IUserAppService userAppService, UserManager userManager)
        {
            _userAppService = userAppService;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Create, AppPermissions.Pages_Administration_Users_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            var output = await _userAppService.GetUserForEdit(new NullableIdInput<long> {Id = id});
            var viewModel = new CreateOrEditUserModalViewModel(output);

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task<PartialViewResult> PermissionsModal(long id)
        {
            var user = await _userManager.GetUserByIdAsync(id);
            var output = await _userAppService.GetUserPermissionsForEdit(new IdInput<long>(id));
            var viewModel = new UserPermissionsEditViewModel(output, user);

            return PartialView("_PermissionsModal", viewModel);
        }
    }
}