﻿using System.Web.Mvc;
using Abp.Application.Navigation;
using Abp.Configuration.Startup;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using KyleTanczos.TestKyle.Sessions;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Layout;
using KyleTanczos.TestKyle.Web.Areas.Mpa.Startup;
using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    [AbpMvcAuthorize]
    public class LayoutController : TestKyleControllerBase
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IUserNavigationManager _userNavigationManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;


        public LayoutController(
            ISessionAppService sessionAppService, 
            IUserNavigationManager userNavigationManager, 
            IMultiTenancyConfig multiTenancyConfig)
        {
            _sessionAppService = sessionAppService;
            _userNavigationManager = userNavigationManager;
            _multiTenancyConfig = multiTenancyConfig;
        }

        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations()),
                Languages = LocalizationManager.GetAllLanguages(),
                CurrentLanguage = LocalizationManager.CurrentLanguage,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                IsImpersonatedLogin = AbpSession.ImpersonatorUserId.HasValue
            };

            return PartialView("_Header", headerModel);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebar(string currentPageName = "")
        {
            string url = this.Request.Url.AbsolutePath;

            var sidebarModel = new SidebarViewModel
            {
           
            
                Menu = (url.Contains("PcrForm") ?
                 AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(Mpa2NavigationProvider.MenuName, AbpSession.UserId))           
                : AsyncHelper.RunSync(() => _userNavigationManager.GetMenuAsync(MpaNavigationProvider.MenuName, AbpSession.UserId))                
                ),
                
                CurrentPageName = currentPageName
            };


            return PartialView("_Sidebar", sidebarModel);
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations())
            };

            return PartialView("_Footer", footerModel);
        }
    }
}