using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{
    public class AgencyInfoDto
    {
        public string AgencyName { get; set; }
        public string AgencyNumber { get; set; }
        public string OrganizationType { get; set; }
        public string OrganizationStatus { get; set; }
        public string States{ get; set; }
        public string Counties { get; set; }
        public string OtherAgenciesInArea { get; set; }
        public string TimeZone { get; set; }
        public string PrimaryTypeOfService { get; set; }
        public string OtherTypesOfService { get; set; }
        public string NationalProviderIdentifier { get; set; }
        public string LevelOfService { get; set; }
        public string AgencyContactZip { get; set; }
        public string ZoneNumbers { get; set; }

    }

    public class AgencyInfoAppService : TestKyleAppServiceBase, IAgencyInfoAppService
    {
        private readonly ISettingsManager _settingsManager;

        public AgencyInfoAppService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public AgencyInfoDto Get()
        {
            var agencyInfo = _settingsManager.GetSettingsOptions<AgencyInfoDto>("D01");
            return agencyInfo;
        }

        public async Task<AgencyInfoDto> Update(AgencyInfoDto agencyInfoDto)
        {
            agencyInfoDto = await _settingsManager.SetSettingsOptions<AgencyInfoDto>(agencyInfoDto, "D01");
            return agencyInfoDto;
        }
    }
}
