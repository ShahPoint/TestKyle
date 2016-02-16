using EntityFramework.DynamicFilters;
using KyleTanczos.TestKyle.EntityFramework;

namespace KyleTanczos.TestKyle.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly TestKyleDbContext _context;

        public TestDataBuilder(TestKyleDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new TestOrganizationUnitsBuilder(_context).Create();

            _context.SaveChanges();
        }
    }
}
