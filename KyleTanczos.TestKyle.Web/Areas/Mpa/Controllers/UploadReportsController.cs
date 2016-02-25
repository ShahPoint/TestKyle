using KyleTanczos.TestKyle.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class UploadReportsController : TestKyleControllerBase
    {
        // GET: Mpa/UploadReports
        public ActionResult Index()
        {
            return View();
        }
    }
}