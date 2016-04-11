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

namespace KyleTanczos.TestKyle.Settings
{
    public class StationsDto
    {
        public string Name { get; set; }
        public string Number { get; set; }
    }

    public class StationsAppService : TestKyleAppServiceBase, IStationsAppService
    {
        private readonly IRepository<Stations> _stationsRepository;

        public StationsAppService(IRepository<Stations> stationsRepository)
        {
            _stationsRepository = stationsRepository;
        }

        public async Task<List<StationsDto>> Get()
        {
            OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            Stations stations = _stationsRepository.GetAll().FirstOrDefault(x => x.OrganizationUnitId == org.Id);
            List<StationsDto> stationsList = new List<StationsDto>() { };
            

            if (stations != null)
            {
                stationsList = JsonConvert.DeserializeObject<List<StationsDto>>(stations.OptionsAsJson);
            }

            return stationsList;
        }


        public async Task<List<StationsDto>> Update(List<StationsDto> stationsDto)
        {
            OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            Stations stations = _stationsRepository.GetAll().FirstOrDefault(x => x.OrganizationUnitId == org.Id);

            string stationsAsJson = JsonConvert.SerializeObject(stationsDto);

            if (stations != null)
            {
                stations.OptionsAsJson = stationsAsJson;
                stations = await _stationsRepository.UpdateAsync(stations);
            }
            else
            {
                stations = new Stations() { OptionsAsJson = stationsAsJson, OrganizationUnitId = org.Id };
                stations = await _stationsRepository.InsertAsync(stations);
            }
            return stationsDto;
        }
    }
}
