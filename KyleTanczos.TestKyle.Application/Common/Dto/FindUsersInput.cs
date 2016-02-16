using KyleTanczos.TestKyle.Dto;

namespace KyleTanczos.TestKyle.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }
    }
}