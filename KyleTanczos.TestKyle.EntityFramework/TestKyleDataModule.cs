using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;

namespace KyleTanczos.TestKyle
{
    /// <summary>
    /// Entity framework module of the application.
    /// </summary>
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(TestKyleCoreModule))]
    public class TestKyleDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            //web.config (or app.config for non-web projects) file should containt a connection string named "Default".
            Configuration.DefaultNameOrConnectionString = "KyleIsABoss";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
