using Abp.Dependency;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using KyleTanczos.TestKyle.Authorization.Users;

namespace KyleTanczos.TestKyle.Web.Auth
{
    public class ApplicationSignInManager : SignInManager<User, long>, ITransientDependency
    {
        public ApplicationSignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}