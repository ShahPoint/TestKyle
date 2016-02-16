using EntityFramework.DynamicFilters;
using KyleTanczos.TestKyle.EntityFramework;

namespace KyleTanczos.TestKyle.Migrations.Seed
{
    public class InitialDbBuilder
    {
        private readonly TestKyleDbContext _context;

        public InitialDbBuilder(TestKyleDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new DefaultTenantRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
