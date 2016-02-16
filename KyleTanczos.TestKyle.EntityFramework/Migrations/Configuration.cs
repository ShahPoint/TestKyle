using System.Data.Entity.Migrations;
using KyleTanczos.TestKyle.Migrations.Seed;

namespace KyleTanczos.TestKyle.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TestKyle.EntityFramework.TestKyleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TestKyle";
        }

        protected override void Seed(TestKyle.EntityFramework.TestKyleDbContext context)
        {
            new InitialDbBuilder(context).Create();
        }
    }
}
