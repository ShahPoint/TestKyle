namespace KyleTanczos.TestKyle.Web
{
    public interface IWebUrlService
    {
        string GetSiteRootAddress(string tenancyName = null);
    }
}
