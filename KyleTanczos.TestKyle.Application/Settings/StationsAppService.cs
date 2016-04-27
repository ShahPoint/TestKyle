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
    public class StationsDto
    {
        public string Name { get; set; }
        public string Number { get; set; }
    }

    public class StationsAppService : TestKyleAppServiceBase, IStationsAppService
    {       
        private readonly ISettingsManager _settingsManager;

        public StationsAppService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public List<StationsDto> Get()
        {
            //OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            //Stations stations = _stationsRepository.GetAll().FirstOrDefault(x => x.OrganizationUnitId == org.Id);
            //List<StationsDto> stationsList = new List<StationsDto>() { };

            var stations = _settingsManager.GetSettingsOptions<StationsDto>("D05");    
            return stations;
        }


        public async Task<List<StationsDto>> Update(List<StationsDto> stationsDto)
        {

           stationsDto =  await _settingsManager.SetSettingsOptions<List<StationsDto>>(stationsDto, "D05");
           return stationsDto;
        }
    }
}
