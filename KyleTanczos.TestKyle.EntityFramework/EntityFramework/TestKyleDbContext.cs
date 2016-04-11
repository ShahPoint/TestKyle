using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using KyleTanczos.TestKyle.Authorization.Roles;
using KyleTanczos.TestKyle.Authorization.Users;
using KyleTanczos.TestKyle.MultiTenancy;
using KyleTanczos.TestKyle.Storage;
using KyleTanczos.TestKyle.Settings;
using KyleTanczos.TestKyle.PcrForm;
using System.Data.Entity.Infrastructure;

namespace KyleTanczos.TestKyle.EntityFramework
{
    public class TestKyleDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */
        public virtual IDbSet<AgencyInfo> AgencyInfos { get; set; }
        public virtual IDbSet<AgencyOption> AgencyOptions { get; set; }
        public virtual IDbSet<Medication> Medications { get; set; }
        public virtual IDbSet<Procedure> Procedures { get; set; }
        public virtual IDbSet<Protocol> Protocols { get; set; }
        public virtual IDbSet<Stations> Stations { get; set; }
        public virtual IDbSet<Vehicle> Vehicles { get; set; }
        public virtual IDbSet<Hospital> Hospitals { get; set; }
        public virtual IDbSet<OtherFacility> OtherFacilities { get; set; }
        public virtual IDbSet<Insurance> Insurances { get; set; }

        public virtual IDbSet<NarrativeGenerator> NarrativeGenerators { get; set; }
        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }
        public IDbSet<NemsisDataElement> NemsisDataElements { get; set; }
        public IDbSet<Select2OptionsList> Select2OptionsList { get; set; }
        public IDbSet<UploadedFile> UploadedFiles { get; set; }
        //public DbSet<PcrPaNemsis> PcrPaNemsises { get; set; }
        //public DbSet<OutComeType> OutcomeTypes { get; set; }
        public IDbSet<RawFile> blobFiles { get; set; }

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public TestKyleDbContext()
            : base("KyleIsABoss")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 1200; //20 minutes
        }

        /* This constructor is used by ABP to pass connection string defined in TestKyleDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of TestKyleDbContext since ABP automatically handles it.
         */
        public TestKyleDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 1200; //20 minutes
        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public TestKyleDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }
    }
}
