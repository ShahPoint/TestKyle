using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;

namespace KyleTanczos.TestKyle.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ApplicationController : TestKyleControllerBase
    {
        [DisableAuditing]
        public ActionResult Index()
        {
            return View("~/App/common/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}