using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using KyleTanczos.TestKyle.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Abp.Application.Services.Dto;
using System.Data.Entity;

namespace KyleTanczos.TestKyle.Settings
{
    public class VehiclesDto
    {
        public string VehicleNumber { get; set; }
        public string CallSign { get; set; }
    }

    public class VehiclesAppService : TestKyleAppServiceBase, IVehiclesAppService
    {

        private readonly ISettingsManager _settingsManager;

        public VehiclesAppService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public List<VehiclesDto> Get()
        {
            var vehicles = _settingsManager.GetSettingsOptions<List<VehiclesDto>>("D06");
            return vehicles;
        }

        public async Task<List<VehiclesDto>> Update(List<VehiclesDto> vehiclesDto)
        {
            vehiclesDto = await _settingsManager.SetSettingsOptions<List<VehiclesDto>>(vehiclesDto, "D06");
            return vehiclesDto;
        }
    }
}
