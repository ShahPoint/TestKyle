using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class DashboardController : TestKyleControllerBase
    {

        [OutputCache(Duration = 300, VaryByParam = "None")]
        public ActionResult Index()
        {
            return View();
        }
    }
}