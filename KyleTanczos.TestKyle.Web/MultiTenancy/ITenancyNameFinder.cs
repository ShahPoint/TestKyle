namespace KyleTanczos.TestKyle.Web.MultiTenancy
{
    public interface ITenancyNameFinder
    {
        string GetCurrentTenancyNameOrNull();
    }
}