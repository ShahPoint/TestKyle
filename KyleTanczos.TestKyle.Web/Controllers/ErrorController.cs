using System.Web.Mvc;
using Abp.Auditing;

namespace KyleTanczos.TestKyle.Web.Controllers
{
    public class ErrorController : TestKyleControllerBase
    {
        [DisableAuditing]
        public ActionResult E404()
        {
            return View();
        }
    }
}