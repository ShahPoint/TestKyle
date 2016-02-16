using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class WelcomeController : TestKyleControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}