using Abp.Dependency;

namespace KyleTanczos.TestKyle
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }

        public string SampleProfileImagesFolder { get; set; }
    }
}