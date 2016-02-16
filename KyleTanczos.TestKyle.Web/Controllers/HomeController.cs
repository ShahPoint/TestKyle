using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Controllers
{
    public class HomeController : TestKyleControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "Mpa" });
        }
	}
}