using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using KyleTanczos.TestKyle.Authorization.Roles;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.MultiTenancy;
using KyleTanczos.TestKyle.Storage;

namespace KyleTanczos.TestKyle.EntityFramework
{
    public class TestKyleDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */

        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public TestKyleDbContext()
            : base("KyleIsABoss")
        {

        }

        /* This constructor is used by ABP to pass connection string defined in TestKyleDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of TestKyleDbContext since ABP automatically handles it.
         */
        public TestKyleDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public TestKyleDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }
    }
}
