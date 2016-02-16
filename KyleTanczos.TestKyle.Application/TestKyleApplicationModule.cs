using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using KyleTanczos.TestKyle.Authorization;

namespace KyleTanczos.TestKyle
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(typeof(TestKyleCoreModule), typeof(AbpAutoMapperModule))]
    public class TestKyleApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Custom DTO auto-mappings
            CustomDtoMapper.CreateMappings();
        }
    }
}
