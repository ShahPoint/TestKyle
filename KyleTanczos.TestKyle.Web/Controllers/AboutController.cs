using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Controllers
{
    public class AboutController : TestKyleControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}