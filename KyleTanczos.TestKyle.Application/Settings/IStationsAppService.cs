using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KyleTanczos.TestKyle.Settings;
using Abp.Web.Models;
//using KyleTanczos.TestKyle.Web.Controllers;

namespace KyleTanczos.TestKyle.Settings
{
    [DontWrapResult]
    public interface IStationsAppService: IApplicationService
    {
        Task<List<StationsDto>> Get();

        //Task<List<StationsDto>> CreateOrUpdateStations(List<StationsDto> stationsDto);
        Task<List<StationsDto>> Update(List<StationsDto> stationsDto);

       // Task<List<Select2Option>> Update(List<StationsDto> stationsDto);

    }
}
