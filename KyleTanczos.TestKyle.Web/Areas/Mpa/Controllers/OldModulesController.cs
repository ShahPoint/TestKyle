using KyleTanczos.TestKyle.EntityFramework;
using KyleTanczos.TestKyle.Web.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Web.UI;
using System.Web.Http;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class NarrativeGenerator {}
    public class Signatures { }
    public class CustomForms { }
    public class Attachments { }
    public class Settings { }
    public class OfflineLogin { }
    public class PcrSaving { }

    public class OldModulesController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }
        
    }

    public class SampleDataController : ApiController
    {
        [System.Web.Http.Route("api/Sync")]
        public bool Get(string header)
        {
            
            return true;
        }
    }
}
