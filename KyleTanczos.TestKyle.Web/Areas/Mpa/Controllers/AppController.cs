using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class AppController : Controller
    {
        // GET: Mpa/App/
        public ActionResult Install()
        {
            return View();
        }
    }
}