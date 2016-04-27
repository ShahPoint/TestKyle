using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KyleTanczos.TestKyle.PcrForm
{

    // this table would be per agency and strings would be 
    //updated (triggering a install of that resource) at our own choosing which
    // would allow us to granular control when different states, agencies, etc are updated
    // and/or things like pcr
    public class InstallTimeStamps
    {
        public int Id { get; set; } 
        public string OfflineAppFiles{ get; set; }
        public string Medications { get; set; }
        public string FipsEntriesForCounties { get; set; }
        public string AutoFipsForCounties { get; set; }
        public string HospitalListForCounties { get; set; }
        public string PcrDropDownOptions { get; set; } // this might be updated every time they install
        public string AgencyDemographic { get; set; }
    }

    public class NemsisDataElement: Entity
    {
        public NemsisDataElement()
        {
            Active = true;
        }

        public int Id { get; set; }
        //Change to NemsisCode
        public string FieldNumber { get; set; } // Example E01_01
        public string FieldName { get; set; } // Incident Number
        public string OptionCode { get; set; }
        public string OptionText { get; set; }
        public string State { get; set; }
        bool Active { get; set; }
    }

    public class Select2OptionsList: Entity
    {
        public string ControlName { get; set; }
        public string oldJsListName { get; set; }
        public string Association { get; set; } // Default, 2 digit state, or orgToken
        public string OptionsAsJson { get; set; }
        [DefaultValue(true)]
        public bool Active { get; set; }
    }

    public class Select2Option
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class UploadedFile
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByUserName { get; set; }
        public int CreatedByUserId { get; set; }
        public string FileName { get; set; }
        public DateTime StartDateRange { get; set; }
        public DateTime EndDateRange { get; set; }
        public int TripCount { get; set; }
        //public List<PcrPaNemsis> Pcrs { get; set; }
        public RawFile RawFile { get; set; }
        public int ByteCount { get; set; }
    }



    public class RawFile
    {
        public int Id { get; set; }
        public byte[] FileContents { get; set; }
        //public int UploadedFileId { get; set; }

    }

    //public class UploadedPcr
    //{
    //    public int Id { get; set; }
    //    public DateTime IncidentDate { get; set; }
    //    public OutComeType outcome { get; set; }
    //}

    //public class OutComeType
    //{
    //    public int Id { get; set; }
    //    public string OutcomeName { get; set; }
    //}

}