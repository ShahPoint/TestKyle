using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.MultiTenancy;

namespace KyleTanczos.TestKyle.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
