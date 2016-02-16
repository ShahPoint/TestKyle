using Abp.Modules;
using Abp.Zero.Configuration;

namespace KyleTanczos.TestKyle.Tests
{
    [DependsOn(
        typeof(TestKyleApplicationModule),
        typeof(TestKyleDataModule))]
    public class TestKyleTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database as language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
        }
    }
}
