using Abp.Application.Services;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{
    [DontWrapResult]
    public interface IMedicationsAppService : IApplicationService
    {
        Task<List<MedicationsDto>> Get();

        //Task<List<StationsDto>> CreateOrUpdateStations(List<StationsDto> stationsDto);
        Task<List<MedicationsDto>> Update(List<MedicationsDto> stationsDto);

    }
}
