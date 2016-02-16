using Abp.WebApi.Controllers;

namespace KyleTanczos.TestKyle.WebApi
{
    public abstract class TestKyleApiControllerBase : AbpApiController
    {
        protected TestKyleApiControllerBase()
        {
            LocalizationSourceName = TestKyleConsts.LocalizationSourceName;
        }
    }
}