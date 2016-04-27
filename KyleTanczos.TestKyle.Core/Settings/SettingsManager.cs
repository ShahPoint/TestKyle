using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using KyleTanczos.TestKyle.PcrForm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleTanczos.TestKyle.Settings
{
    public class SettingsManager : DomainService, ISettingsManager
    {
        private readonly IRepository<NemsisDataElement> _nemsisDataElementRepository;
        private readonly IRepository<Select2OptionsList> _select2OptionsListRepository;
        private readonly IRepository<Stations> _stationsRepository;

        public SettingsManager(IRepository<Stations> stationsRepository, IRepository<Select2OptionsList> select2OptionsListRepository, IRepository<NemsisDataElement> nemsisDataElementRepository)
        {
            _stationsRepository = stationsRepository;
            _select2OptionsListRepository = select2OptionsListRepository;
            _nemsisDataElementRepository = nemsisDataElementRepository;
        }

        public List<T> GetSettingsOptions<T>(string NemsisElement)
        {

            //try
            //{
                //First check the custom options for any custom options created by the org
                var agencyCustomOptions = _select2OptionsListRepository.GetAll().FirstOrDefault(x => x.Association == "1" && x.ControlName == NemsisElement && x.Active == true);
                if (agencyCustomOptions != null)
                {
                    var optionsAsJson = agencyCustomOptions.OptionsAsJson;
                    return JsonConvert.DeserializeObject<List<T>>(optionsAsJson);
                }

                //Then check if the state has options 
                var stateCustomOptions = _nemsisDataElementRepository.GetAll().Where(x => x.FieldNumber == NemsisElement && x.State == "PA");
                if (stateCustomOptions != null)
                {
                    return stateCustomOptions.Select(x => new Select2Option() { id = x.OptionText, text = x.OptionText }).ToList() as List<T>;
                }

                //Else return the default values
                var defaultCustomOptions = _nemsisDataElementRepository.GetAll().Where(x => x.FieldNumber == NemsisElement && x.State == "DEFAULT");
                if (defaultCustomOptions != null)
                {
                    return defaultCustomOptions.Select(x => new Select2Option() { id = x.OptionText, text = x.OptionText }).ToList() as List<T>;
                }

                return new List<T>();
            //}
            //catch (Exception ex)
            //{
            //    return new List<T>() { };
            //}
        }

        public async Task<T> SetSettingsOptions<T> (T SettingsOption, string NemsisElement)
        {       
            //Grab all old custom values for this row and turn them off so only one is active
            var oldOptions = _select2OptionsListRepository.GetAll().Where(x => x.Association == "1" && x.ControlName == NemsisElement && x.Active == true).ToList();
            
            //Set all variables to have active = false
            //Should only be one unless someone messed up somewhere
            foreach(Select2OptionsList oldOption in oldOptions)
            {
                oldOption.Active = false;
                await _select2OptionsListRepository.UpdateAsync(oldOption);
            }

            //Create New Settings Option 
            Select2OptionsList options = new Select2OptionsList()
            {
                Active = true,
                Association = "1",
                ControlName = NemsisElement,
                OptionsAsJson = JsonConvert.SerializeObject(SettingsOption),
                oldJsListName = ""
            };

            //Save Options to DB
            options = await _select2OptionsListRepository.InsertAsync(options);


            return SettingsOption;
        }    
    }

    public interface ISettingsManager: IDomainService
    {
        List<T> GetSettingsOptions<T>(string NemsisElement);
        Task<T> SetSettingsOptions<T>(T SettingsOption, string NemsisElement);
    }
}
