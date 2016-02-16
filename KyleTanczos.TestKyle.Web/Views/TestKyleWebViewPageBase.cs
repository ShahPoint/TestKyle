using Abp.Web.Mvc.Views;

namespace KyleTanczos.TestKyle.Web.Views
{
    public abstract class TestKyleWebViewPageBase : TestKyleWebViewPageBase<dynamic>
    {

    }

    public abstract class TestKyleWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected TestKyleWebViewPageBase()
        {
            LocalizationSourceName = TestKyleConsts.LocalizationSourceName;
        }
    }
}