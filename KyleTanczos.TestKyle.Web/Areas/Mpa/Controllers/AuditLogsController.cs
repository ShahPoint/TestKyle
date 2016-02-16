using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : TestKyleControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}