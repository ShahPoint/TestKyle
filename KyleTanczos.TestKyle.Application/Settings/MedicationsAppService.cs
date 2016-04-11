using Abp.Domain.Repositories;
using Abp.Organizations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{
    public class MedicationsDto
    {
        public string Name { get; set; }
        public string CertificationLevel { get; set; }
    }

    public class MedicationsAppService : TestKyleAppServiceBase, IMedicationsAppService
    {
        private readonly IRepository<Medication> _medicationsRepository;

        public MedicationsAppService(IRepository<Medication> medicationsRepository)
        {
            _medicationsRepository = medicationsRepository;
        }

        public async Task<List<MedicationsDto>> Get()
        {
            OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            Medication medications = _medicationsRepository.GetAll().Where(x => x.OrganizationUnitId == org.Id).OrderByDescending(x => x.LastModificationTime).FirstOrDefault();
            List<MedicationsDto> medicationsList = new List<MedicationsDto>() { };


            if (medications != null)
            {
                medicationsList = JsonConvert.DeserializeObject<List<MedicationsDto>>(medications.OptionsAsJson);
            }
            else
            {
                medicationsList = new List<MedicationsDto>()
                {
                    new MedicationsDto () { Name = "Name", CertificationLevel = "Certlevel" }
                };
            }

            return medicationsList;
        }


        public async Task<List<MedicationsDto>> Update(List<MedicationsDto> medicationsDto)
        {
            OrganizationUnit org = await GetCurrentOrganizationUnitAsync();
            string medicationsAsJson = JsonConvert.SerializeObject(medicationsDto);

            //Medication medications = _medicationsRepository.GetAll().FirstOrDefault(x => x.OrganizationUnitId == org.Id);       
            //if (medications != null)
            //{
            //    medications.OptionsAsJson = medicationsAsJson;
            //    medications = await _medicationsRepository.UpdateAsync(medications);
            //}
            //else
            //{
            Medication medications = new Medication() { OptionsAsJson = medicationsAsJson, OrganizationUnitId = org.Id };
            medications = await _medicationsRepository.InsertAsync(medications);
            //}
            return medicationsDto;
        }
    }
}
