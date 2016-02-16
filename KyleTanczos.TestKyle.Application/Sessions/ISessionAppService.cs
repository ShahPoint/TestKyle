using System.Threading.Tasks;
using Abp.Application.Services;
using KyleTanczos.TestKyle.Sessions.Dto;

namespace KyleTanczos.TestKyle.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
