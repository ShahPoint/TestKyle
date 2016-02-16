using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.Common.Dto;

namespace KyleTanczos.TestKyle.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultOutput<ComboboxItemDto>> GetEditionsForCombobox();

        Task<PagedResultOutput<NameValueDto>> FindUsers(FindUsersInput input);
    }
}