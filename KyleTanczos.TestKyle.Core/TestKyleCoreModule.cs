using System.Reflection;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Zero;
using Abp.Zero.Configuration;
using Abp.Zero.Ldap;
using Abp.Zero.Ldap.Configuration;
using KyleTanczos.TestKyle.Authorization.Ldap;
using KyleTanczos.TestKyle.Authorization.Roles;
using KyleTanczos.TestKyle.Configuration;
using KyleTanczos.TestKyle.Features;

namespace KyleTanczos.TestKyle
{
    /// <summary>
    /// Core (domain) module of the application.
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule), typeof(AbpZeroLdapModule))]
    public class TestKyleCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Add/remove localization sources
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    "TestKyle",
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "KyleTanczos.TestKyle.Localization.TestKyle"
                        )
                    )
                );

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = false;

            //Enable LDAP authentication (It can be enabled only if MultiTenancy is disabled!)
            //Configuration.Modules.ZeroLdap().Enable(typeof(AppLdapAuthenticationSource));

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

#if DEBUG
            //Disabling email sending in debug mode
            IocManager.Register<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
#endif
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
