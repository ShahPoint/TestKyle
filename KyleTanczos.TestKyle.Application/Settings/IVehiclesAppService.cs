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
    public interface IVehiclesAppService: IApplicationService
    {
        List<VehiclesDto> Get();

        Task<List<VehiclesDto>> Update(List<VehiclesDto> vehiclesDto);
    }
}
