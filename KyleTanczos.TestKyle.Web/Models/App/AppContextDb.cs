using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace KyleTanczos.TestKyle.Web.Models.App
{
    public class AppContextDb : DbContext
    {
        public DbSet<NemsisDataElement> NemsisDataElements { get; set; }
        public DbSet<Select2OptionsList> Select2OptionsList { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        //public DbSet<PcrPaNemsis> PcrPaNemsises { get; set; }
        //public DbSet<OutComeType> OutcomeTypes { get; set; }
        public DbSet<RawFile> blobFiles { get; set; }

        public AppContextDb()
            : base("KyleIsABoss")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 1200; //20 minutes
        }

    }
}