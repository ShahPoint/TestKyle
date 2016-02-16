using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Common.Modals;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class CommonController : TestKyleControllerBase
    {
        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }
    }
}