using Abp.Domain.Services;

namespace KyleTanczos.TestKyle
{
    public abstract class TestKyleDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected TestKyleDomainServiceBase()
        {
            LocalizationSourceName = TestKyleConsts.LocalizationSourceName;
        }
    }
}
