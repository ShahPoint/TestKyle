using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace KyleTanczos.TestKyle.Web.Models.App
{
    public class AppContextDb : DbContext
    {
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<UploadedPcr> UploadedPcrs { get; set; }
        public DbSet<OutComeType> OutcomeTypes { get; set; }

        public AppContextDb()
            : base("KyleIsABoss")
        {

        }

    }
}