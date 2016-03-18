using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class NemsisModel
    {
        public string NemsisXml { get; set; }
    }

    public class NemsisTestController : Controller
    {
        // GET: Mpa/NemsisTest
        public ActionResult Index()
        {
        NemsisModel nemsisXML = GetNemsisXml();
            return View( nemsisXML);
        }

        private NemsisModel GetNemsisXml()
        {
            NemsisModel returnString = new NemsisModel();
            returnString.NemsisXml = "yah i am here";

            return returnString;
        }
    }
}