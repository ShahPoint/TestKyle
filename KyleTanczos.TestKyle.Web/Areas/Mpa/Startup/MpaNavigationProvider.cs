using Abp.Application.Navigation;
using Abp.Localization;
using KyleTanczos.TestKyle.Authorization;
using KyleTanczos.TestKyle.Web.Navigation;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Startup
{
    public class Mpa2NavigationProvider : NavigationProvider
    {
        public const string MenuName = "Mpa2";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                 .AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "Mpa/Dashboard",
                    icon: "icon-home",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )

                )
                .AddItem(new MenuItemDefinition(
                    "IncidentTab",
                    L("IncidentTab"),
                    url: "#IncidentTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "PatientTab",
                    L("PatientTab"),
                    url: "#PatientTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "AssessmentTab",
                    L("AssessmentTab"),
                    url: "#AssessmentTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "BillingTab",
                    L("BillingTab"),
                    url: "#BillingTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "TreatmentTab",
                    L("TreatmentTab"),
                    url: "#TreatmentTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "OutcomeTab",
                    L("OutcomeTab"),
                    url: "#OutcomeTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "NarrativeTab",
                    L("NarrativeTab"),
                    url: "#NarrativeTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "DocumentsTab",
                    L("DocumentsTab"),
                    url: "#DocumentsTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "SignaturesTab",
                    L("SignaturesTab"),
                    url: "#SignaturesTab",
                    icon: "icon-grid"
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "NotesTab",
                    L("NotesTab"),
                    url: "#NotesTab",
                    icon: "icon-grid"
                    )
                )
               
                ;


        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TestKyleConsts.LocalizationSourceName);
        }
    }
    public class MpaNavigationProvider : NavigationProvider
    {
        public const string MenuName = "Mpa";
        
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Tenants,
                    L("Tenants"),
                    url: "Mpa/Tenants",
                    icon: "icon-globe",
                    requiredPermissionName: AppPermissions.Pages_Tenants
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Host.Editions,
                    L("Editions"),
                    url: "Mpa/Editions",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Editions
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "Editions2",
                    L("UploadReports"),
                    url: "Mpa/UploadReports",
                    icon: "icon-grid",
                    requiredPermissionName: AppPermissions.Pages_Editions
                    )
                )
                .AddItem(new MenuItemDefinition(
                    "PcrForm",
                    L("PcrForm"),
                    url: "Mpa/PcrForm",
                    icon: "icon-grid"
                    )
                )

                .AddItem(new MenuItemDefinition(
                    PageNames.App.Tenant.Dashboard,
                    L("Dashboard"),
                    url: "Mpa/Dashboard",
                    icon: "icon-home",
                    requiredPermissionName: AppPermissions.Pages_Tenant_Dashboard
                    )
                ).AddItem(new MenuItemDefinition(
                    PageNames.App.Common.Administration,
                    L("Administration"),
                    icon: "icon-wrench"
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Stations,
                        L("Stations"),
                        url: "Mpa/Stations",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Vehicles,
                        L("Vehicles"),
                        url: "Mpa/Vehicles",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )

                    )
                    .AddItem(new MenuItemDefinition(
                        PageNames.App.Common.ManageUsers,
                        L("ManageUsers"),
                        url: "Mpa/ManageUsers",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )

                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.OrganizationUnits,
                        L("OrganizationUnits"),
                        url: "Mpa/OrganizationUnits",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Medications,
                        L("Medications"),
                        url: "Mpa/Medications",
                        icon: "icon-layers",
                        requiredPermissionName: AppPermissions.Pages_Administration_OrganizationUnits
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Roles,
                        L("Roles"),
                        url: "Mpa/Roles",
                        icon: "icon-briefcase",
                        requiredPermissionName: AppPermissions.Pages_Administration_Roles
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Users,
                        L("Users"),
                        url: "Mpa/Users",
                        icon: "icon-users",
                        requiredPermissionName: AppPermissions.Pages_Administration_Users
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.Languages,
                        L("Languages"),
                        url: "Mpa/Languages",
                        icon: "icon-flag",
                        requiredPermissionName: AppPermissions.Pages_Administration_Languages
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Common.AuditLogs,
                        L("AuditLogs"),
                        url: "Mpa/AuditLogs",
                        icon: "icon-lock",
                        requiredPermissionName: AppPermissions.Pages_Administration_AuditLogs
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Host.Settings,
                        L("Settings"),
                        url: "Mpa/HostSettings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Host_Settings
                        )
                    ).AddItem(new MenuItemDefinition(
                        PageNames.App.Tenant.Settings,
                        L("Settings"),
                        url: "Mpa/Settings",
                        icon: "icon-settings",
                        requiredPermissionName: AppPermissions.Pages_Administration_Tenant_Settings
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, TestKyleConsts.LocalizationSourceName);
        }
    }
}