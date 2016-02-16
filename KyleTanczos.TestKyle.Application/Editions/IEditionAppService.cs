using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using KyleTanczos.TestKyle.Editions.Dto;

namespace KyleTanczos.TestKyle.Editions
{
    public interface IEditionAppService : IApplicationService
    {
        Task<ListResultOutput<EditionListDto>> GetEditions();

        Task<GetEditionForEditOutput> GetEditionForEdit(NullableIdInput input);

        Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input);

        Task DeleteEdition(EntityRequestInput input);
    }
}