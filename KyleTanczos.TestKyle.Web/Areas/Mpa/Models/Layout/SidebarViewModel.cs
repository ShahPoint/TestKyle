﻿using Abp.Application.Navigation;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Models.Layout
{
    public class SidebarViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }
}