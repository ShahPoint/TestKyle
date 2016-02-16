using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Configuration.Host;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsController : TestKyleControllerBase
    {
        private readonly IHostSettingsAppService _hostSettingsAppService;

        public HostSettingsController(IHostSettingsAppService hostSettingsAppService)
        {
            _hostSettingsAppService = hostSettingsAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _hostSettingsAppService.GetAllSettings();

            return View(output);
        }
    }
}