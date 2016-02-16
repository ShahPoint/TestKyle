using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace KyleTanczos.TestKyle.WebApi
{
    /// <summary>
    /// Web API layer of the application.
    /// </summary>
    [DependsOn(typeof(AbpWebApiModule), typeof(TestKyleApplicationModule))]
    public class TestKyleWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //Automatically creates Web API controllers for all application services of the application
            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(TestKyleApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));
        }
    }
}
