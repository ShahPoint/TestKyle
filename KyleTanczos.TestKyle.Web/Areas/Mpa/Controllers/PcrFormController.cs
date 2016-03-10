using KyleTanczos.TestKyle.EntityFramework;
using KyleTanczos.TestKyle.Web.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Web.UI;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class PcrForm
    {
        public PcrForm()
        {
            Tabs = new List<Tab>();
        }

        public List<Tab> Tabs { get; set; }
    }



    public class Tab
    {
        public Tab()
        {
            PartialTemplateName = "Tab";
            Sections = new List<Section>();
        }
        public string TabTargetName { get; set; }
        public List<Section> Sections { get; set; }
        public string PartialTemplateName { get; set; }
    }

    public class Section
    {
        public Section()
        {
            SectionName = "Title Here";
            Controls = new List<Ctrl>();
            ResponsiveWidth = 12;
            Side = SectionSideEnum.left;
            PartialTemplateName = "Section";

        }
        public string SectionName { get; set; }
        public int ResponsiveWidth { get; set; }
        public List<Ctrl> Controls { get; set; }
        public SectionSideEnum Side { get; set; }
        public string PartialTemplateName { get; set; }
        public Dialog Dialog { get; set; }
        public Toggle Toggle { get; set; }
    }

    public class Toggle
    {
        public Toggle()
        {
            OnText = "Yes";
            OffText = "No";
        }

        public string OnText { get; set; }
        public string OffText { get; set; }
        //public string IsChecked { get; set; }
        public string NgModel { get; set; }
        public string TargetContainerId { get; set; }
    }


    public enum SectionSideEnum { left, right }

    public class Dialog
    {
        public Dialog()
        {
            Controls = new List<Ctrl>();
            DialogTitle = "Title Here";
            SubmitBtnText = "Save";
            CancelBtnText = "Cancel";
        }

        public string DialogTargetId { get; set; }
        public string DialogTitle { get; set; }
        public List<Ctrl> Controls { get; set; }
        public string OnSubmitClick { get; set; }
        public string OnCancelClick { get; set; }
        public string NgSubmitClick { get; set; }
        public string NgCancelClick { get; set; }
        public string SubmitBtnText { get; set; }
        public string CancelBtnText { get; set; }
    }

    public class DropDownList : Ctrl
    {
        public DropDownList()
        {
            DropDownOptions = new List<Select2Option>();
            IsSelect2 = false;
            ControlType = ControlTypeEnum.DropDownList;
        }
        public List<Select2Option> DropDownOptions { get; set; }
        public bool IsSelect2 { get; set; }
        public string GetSelect2Class()
        {
            return (IsSelect2 ? "select2Defualt" : "");
        }
    }

    public class TimePicker : Ctrl
    {
        public TimePicker()
        {
            ControlType = ControlTypeEnum.TimePicker;
        }
    }

    public class TextBox : Ctrl
    {
        public TextBox()
        {
            ControlType = ControlTypeEnum.TextBox;
        }
    }


    public class Ctrl
    {
        public Ctrl()
        {
            ResponsiveWidth = 6; 
        }
        public string DisplayName { get; set; }
        public ControlTypeEnum ControlType { get; set; }

        public int ResponsiveWidth { get; set; }
        public string PlaceHolder { get; set; }
        public string ClientId { get; set; }
        private string _NgModel;

        public string NgModel
        {
            get
            {
                return (string.IsNullOrEmpty(_NgModel) ? ""
                    : "ng-model=pcr." + _NgModel);
            }
            set { _NgModel = value; }
        }

    }

    public static class CtrlHelpers
    {
        public static List<Select2Option> GetSelect2Options(IEnumerable<string> strOptions)
        {
            var returnOptions = strOptions.Select(x => new Select2Option() { id = x, text = x }).ToList();

            return returnOptions;
        }
    }

    public class Select2Option
    {
        public string id { get; set; }
        public string text { get; set; }
    }


    public enum ControlTypeEnum { PatientMeds, MileageBox, TextBox, DropDownList, DropDownListTest, Select2, Select2Single, Select2Many, Select2TagsSingle, Select2TagsMany, TimePicker }

    public class GetPcrFormSelect2Options
    {
        public GetPcrFormSelect2Options(string state, string agencyToken )
        {
            using (AppContextDb appContext = new AppContextDb())
            {
                defaultOptions = appContext.NemsisDataElements.Where(x => x.State == "Default").ToList();
                stateOptions = appContext.NemsisDataElements.Where(x => x.State == state).ToList();
                agencySelect2OptionsLists = appContext.Select2OptionsList.Where(x => x.Association == agencyToken).ToList();
            }
        }

        private List<NemsisDataElement> defaultOptions;
        private List<NemsisDataElement> stateOptions;
        private List<Select2OptionsList> agencySelect2OptionsLists;

        public List<Select2Option> NemsisSelectOptions(string NemsisId)
        {
            var agencySelect2CustomOptions = agencySelect2OptionsLists.FirstOrDefault(x => x.ControlName == NemsisId);

            if (agencySelect2CustomOptions != null)
            {
                var optionsAsJson = agencySelect2CustomOptions.OptionsAsJson;
                return JsonConvert.DeserializeObject<List<Select2Option>>(optionsAsJson);
            }

            var stateSelect2Options = stateOptions.Where(x => x.FieldNumber == NemsisId);

            if (stateSelect2Options.Count() > 0)
            {
                return stateSelect2Options.Select(x => new Select2Option() { id = x.OptionText, text = x.OptionText }).ToList();
            }

            var defaultSelect2Options = defaultOptions.Where(x => x.FieldNumber == NemsisId);
            return defaultSelect2Options.Select(x => new Select2Option() { id = x.OptionText, text = x.OptionText }).ToList();
        }
}

    [OutputCache(Duration = 120, VaryByParam = "none", Location = OutputCacheLocation.Client)]
    // public enum ResponsiveWidthEnum {ng1, ng2, ng3, ng4, ng5, ng6, ng7, ng8, ng9, ng10, ng11, ng12 }
    public class PcrFormController : Controller
    {

        //public void ReadCsvIntoDatabase()
        //{
        //    using (TextFieldParser csvReader = new TextFieldParser(@"C:\GitHubLocal\TestKyle\KyleTanczos.TestKyle.Web\App_Data\NEMSIS_Data_Dictionary_V2_2.csv"))
        //    {
        //        AppContextDb db = new AppContextDb();

        //        csvReader.SetDelimiters(new string[] { "," });
        //        csvReader.HasFieldsEnclosedInQuotes = true;
        //        string[] colFields = csvReader.ReadFields();

        //        while (!csvReader.EndOfData)
        //        {
        //            string[] fieldData = csvReader.ReadFields();
        //            db.NemsisDataElements.Add(new NemsisDataElement()
        //            {
        //                FieldName = fieldData[1],
        //                FieldNumber = fieldData[0],
        //                OptionCode = fieldData[2],
        //                OptionText = fieldData[3],
        //                State = "Default"
        //            });
        //        }

        //        //db.SaveChanges();
        //    }
        //}


        //[OutputCache(Duration = 36000, VaryByParam = "none", Location = OutputCacheLocation.Server)]
        // GET: Mpa/PcrForm
        public ActionResult Index()
        {
            //(Constructor)

            string state = "PA"; // set from user info
            string agencyToken = "Superior"; // set from user info

            GetPcrFormSelect2Options options = new GetPcrFormSelect2Options(state, agencyToken);

            PcrForm pcrForm = new PcrForm()
            {
                Tabs = new List<Tab>()
                {
                    new Tab()
                    {
                        TabTargetName = "IncidentTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                PartialTemplateName = "SectionWithDialog",

                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "IncidentDialog",
                                        DialogTitle = "Incident Modal",
                                        Controls = new List<Ctrl>()
                                        {
                                            new DropDownList() { DisplayName = "Disposition/Outcome2", 
                                                DropDownOptions = options.NemsisSelectOptions("E20_10"), ResponsiveWidth = 12
                                                }

                                        },
                                         OnCancelClick = "alert('cancel')",
                                         OnSubmitClick = "alert('submit')"
                                     
                                },

                                SectionName = "Disposition",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Disposition/Outcome", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_10"), ResponsiveWidth = 12
                                        ,NgModel = "E20_10"
                                        }
                                }
                            },
                            new Section()
                            {
         
                                SectionName = "Incident",
                                Controls = new List<Ctrl>()
                                {
                                   new TextBox() { DisplayName = "Incident Number"
                                   ,NgModel = "E02_02"
                                        },
                                    new DropDownList() { DisplayName = "Response Urgency", ControlType = ControlTypeEnum.DropDownListTest,
                                        DropDownOptions = options.NemsisSelectOptions("E07_33")
                                        ,NgModel = "E07_33"
                                        },
                                    new DropDownList() { DisplayName = "CMS Level", ControlType = ControlTypeEnum.DropDownListTest,
                                        DropDownOptions = options.NemsisSelectOptions("D01_06")
                                        ,NgModel = "D01_06"
                                        },
                                    new DropDownList() { DisplayName = "Type Of Location", 
                                        DropDownOptions = options.NemsisSelectOptions("E08_07")
                                        ,NgModel = "E08_07"
                                        },
                                    new DropDownList() { DisplayName = "Nature Of Incident", 
                                        DropDownOptions = options.NemsisSelectOptions("E03_01")
                                        ,NgModel = "E03_01"
                                        },
                                    new TextBox() { DisplayName = "Scene Address"
                                        ,NgModel = ""
                                        }

                                }
                            },
                            new Section()
                            {    SectionName = "Dispatch",
                                Controls = new List<Ctrl>()
                                {
                                   new DropDownList() { DisplayName = "Call Sign", 
                                        DropDownOptions = options.NemsisSelectOptions("E02_12")
                                        ,NgModel = "E02_12"
                                        },
                                        new DropDownList() { DisplayName = "Vehicle Number", 
                                        DropDownOptions = options.NemsisSelectOptions("E02_11")
                                        ,NgModel = "E02_11"
                                        },
                                    new DropDownList() { DisplayName = "Mode To Scene", 
                                        DropDownOptions = options.NemsisSelectOptions("E02_20")
                                        ,NgModel = "E02_20"
                                        },
                                        new TextBox() { DisplayName = "Veh. Incident #"
                                        ,NgModel = ""
                                        },
                                    new DropDownList() { DisplayName = "Service Requested", 
                                        DropDownOptions = options.NemsisSelectOptions("E02_04")
                                        ,NgModel = "E02_04"
                                        },
                                    new DropDownList() { DisplayName = "Role", 
                                        DropDownOptions = options.NemsisSelectOptions("E02_05")
                                        ,NgModel = "E02_05"
                                        }
                                }
                            },
                            new Section()
                            {
                                PartialTemplateName = "SectionWithToggleCtrl",
                                Toggle = new Toggle()
                                {
                                     TargetContainerId = "OthersOnScene"
                                },
                                SectionName = "Others On Scene",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Services On Scene", 
                                        DropDownOptions = options.NemsisSelectOptions("E08_02")
                                        ,NgModel = "E08_02"
                                        },
                                    new DropDownList() { DisplayName = "Other EMS Agencies", 
                                        DropDownOptions = options.NemsisSelectOptions("E08_01")
                                        ,NgModel = "E08_01"
                                        },
                                    new TextBox() { DisplayName = "EMS System"
                                        ,NgModel = "D04_17"
                                        },
                                    new TextBox() { DisplayName = "Response Differential"
                                    ,NgModel = "E08_03"
                                    }
                                }
                            }
                            ,
                            new Section()
                            {


                                Side = SectionSideEnum.right,
                                SectionName = "Times",
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new TimePicker() { DisplayName = "Incident Date",  ResponsiveWidth = 12
                                        ,NgModel = ""
                                   },
                                   new TimePicker() { DisplayName = "Onset",  ResponsiveWidth = 12
                                   ,NgModel = "E05_01"
                                   },
                                    new TimePicker() { DisplayName = "Recieved",   ResponsiveWidth = 12
                                        ,NgModel = "E05_02"},
                                   new TimePicker() { DisplayName = "Notified",  ResponsiveWidth = 12
                                        ,NgModel = "E05_03"},
                                    new TimePicker() { DisplayName = "Dispatched", ResponsiveWidth = 12
                                        ,NgModel = "E05_04"},
                                     new TimePicker() { DisplayName = "Enroute",  ResponsiveWidth = 12
                                        ,NgModel = "E05_05"},
                                    new TimePicker() { DisplayName = "Arrival", ResponsiveWidth = 12
                                        ,NgModel = "E05_06"},
                                    new TimePicker() { DisplayName = "Contacted", ResponsiveWidth = 12
                                        ,NgModel = "E05_07"},
                                    new TimePicker() { DisplayName = "Transfer", ResponsiveWidth = 12
                                        ,NgModel = "E05_08"},
                                    new TimePicker() { DisplayName = "Departed", ResponsiveWidth = 12
                                        ,NgModel = "E05_09"},
                                    new TimePicker() { DisplayName = "Arrival", ResponsiveWidth = 12
                                        ,NgModel = "E05_10"},
                                    new TimePicker() { DisplayName = "Available", ResponsiveWidth = 12
                                        ,NgModel = "E05_11"},
                                    new TimePicker() { DisplayName = "At Base", ResponsiveWidth = 12
                                        ,NgModel = "E05_13"},
                                    new TimePicker() { DisplayName = "Cancelled", ResponsiveWidth = 12
                                        ,NgModel = "E05_12"},
                                }
                            },
                            new Section()
                            {
                                SectionName = "Crew",
                                Side = SectionSideEnum.right,
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new DropDownList() { DisplayName = "Primary", 
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = "E04_02"},
                                    new DropDownList() { DisplayName = "Secondary", 
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = ""},
                                    new DropDownList() { DisplayName = "Third", 
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = ""},
                                    new DropDownList() { DisplayName = "Other", 
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = ""}
                                }
                            },
                            new Section()
                            {
                                SectionName = "Mileage",
                                Side = SectionSideEnum.right,
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new TextBox() { DisplayName = "Start", ResponsiveWidth = 12
                                        ,NgModel = "E02_16"},
                                    new TextBox() { DisplayName = "Scene", ResponsiveWidth = 12
                                        ,NgModel = "E02_17"},
                                    new TextBox() { DisplayName = "Dest.", ResponsiveWidth = 12
                                        ,NgModel = "E02_18"},
                                    new TextBox() { DisplayName = "Service", ResponsiveWidth = 12
                                        ,NgModel = "E02_19"}
                                }
                            }


                        }
                    },
                    new Tab()
                    {
                         TabTargetName = "PatientTab",
                         Sections = new List<Section>()
                         {
                             new Section()
                             {
                                 SectionName = "Patient Info",
                                 Controls = new List<Ctrl>()
                                 {
                                    new TextBox() { DisplayName = "First Name"
                                       ,NgModel = "E06_02"},
                                    new TextBox() { DisplayName = "Phone"
                                       ,NgModel = "E06_17"},
                                    new TextBox() { DisplayName = "Last Name"
                                       ,NgModel = "E06_01"},
                                    new TextBox() { DisplayName = "Weight"
                                       ,NgModel = "E16_01"},
                                    new TextBox() { DisplayName = "M.I."
                                       ,NgModel = "E06_03"},
                                    new DropDownList() { DisplayName = "Race", 
                                        DropDownOptions = options.NemsisSelectOptions("E06_12")
                                       ,NgModel = "E06_12"},
                                    new TextBox() { DisplayName = "DOB"
                                       ,NgModel = "E06_16"},
                                    new DropDownList() { DisplayName = "Ethnicity", 
                                        DropDownOptions = options.NemsisSelectOptions("E06_13")
                                       ,NgModel = "E06_13"},
                                    new DropDownList() { DisplayName = "Gender", 
                                        DropDownOptions = options.NemsisSelectOptions("E06_11")
                                       ,NgModel = "E06_11"},
                                    new TextBox() { DisplayName = "SSN"
                                       ,NgModel = "E06_10"},
                                    new TextBox() { DisplayName = "Pattient Address"
                                       ,NgModel = ""}
                                    }
                             },
                             new Section()
                             {
                                 SectionName = "Personal",
                                 Controls = new List<Ctrl>()
                                 {

                                    new TextBox() { DisplayName = "DL Number"
                                       ,NgModel = "E06_19"},
                                    new TextBox() { DisplayName = "Pt Practitioner Name"
                                       ,NgModel = "E06_18"},
                                     new DropDownList() { DisplayName = "DL State", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_06")
                                       ,NgModel = "E12_06"}
                                 }
                             },
                             new Section()
                             {
                                 SectionName = "Medical Info",
                                 Side = SectionSideEnum.right,
                                 Controls = new List<Ctrl>()
                                 {
                                     new DropDownList() { DisplayName = "History", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_10")
                                       ,NgModel = "E12_10"},
                                    new DropDownList() { DisplayName = "History Obtained", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_11")
                                       ,NgModel = "E12_11"},
                                    new DropDownList() { DisplayName = "Allergies (Meds)", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_08")
                                       ,NgModel = "E12_08"},
                                    new DropDownList() { DisplayName = "Emergency Form", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_18")
                                       ,NgModel = "E12_18" },
                                    new DropDownList() { DisplayName = "Allergies (Other)", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_09")
                                       ,NgModel = "E12_09" },
                                    new DropDownList() { DisplayName = "Advanced Directives", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_07")
                                       ,NgModel = "E12_07" },
                                    new DropDownList() { DisplayName = "Triage Color", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "CustomTriageColor" },
                                    new DropDownList() { DisplayName = "Triage Category", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "CustomTriageCategory" },
                                    new DropDownList() { DisplayName = "Pregnant?", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_20")
                                       ,NgModel = "E12_20" },
                                    new DropDownList() { DisplayName = "# Past Pregnancies", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "" }
                                 }

                             },
                             new Section()
                             {
                                 SectionName = "Medications",
                                 Side = SectionSideEnum.right,
                                 Controls = new List<Ctrl>()
                                 {
                                     new Ctrl()
                                     {
                                        ControlType = ControlTypeEnum.PatientMeds
                                     }
                                 }

                             }
                         }

                    },
                    new Tab()
                    {
                        TabTargetName = "AssessmentTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                SectionName = "Patient Complaints",
                                Controls = new List<Ctrl>()
                                {
                                    new TextBox() { DisplayName = "Chief Complaint"
                                       ,NgModel = "E09_05" },
                                    new TextBox() { DisplayName = "Duration", ResponsiveWidth = 3
                                       ,NgModel = "E09_06" },
                                    new DropDownList() { DisplayName = "Units", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_07"), ResponsiveWidth = 3
                                       ,NgModel = "E09_07" },
                                    new TextBox() { DisplayName = "Secondary Complaint"
                                       ,NgModel = "E09_08" },
                                    new TextBox() { DisplayName = "Duration", ResponsiveWidth = 3
                                       ,NgModel = "E09_09" },
                                    new DropDownList() { DisplayName = "Units", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_10"), ResponsiveWidth = 3
                                       ,NgModel = "E09_10" },
                                    new DropDownList() { DisplayName = "Barriers To Patient Care", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_01"), ResponsiveWidth = 12
                                       ,NgModel = "E12_01" }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Exams",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Patient Complaints", ResponsiveWidth = 12 }

                                }
                            },
                            new Section()
                            {   PartialTemplateName = "SectionWithToggleCtrl",
                                Toggle = new Toggle()
                                {
                                     TargetContainerId = "VehicleCollision"
                                },
                                SectionName = "Vehicle Collision",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Vehicle Collision Impact", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_05"), ResponsiveWidth = 12
                                       ,NgModel = "E10_05" },
                                    new TextBox() { DisplayName = "Report Number"
                                       ,NgModel = "E22_03" },
                                    new DropDownList() { DisplayName = "Pt Location", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_07")
                                       ,NgModel = "E10_07" },
                                    new DropDownList() { DisplayName = "Row", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_06")
                                       ,NgModel = "E10_06" },
                                    new DropDownList() { DisplayName = "Safety Equipment", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_08")
                                       ,NgModel = "E10_08" },
                                    new DropDownList() { DisplayName = "Airbags", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_09")
                                       ,NgModel = "E10_09" },
                                    new DropDownList() { DisplayName = "Injury Indicators", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_04")
                                       ,NgModel = "E10_04" }
                                }
                            },
                            new Section()
                            {   PartialTemplateName = "SectionWithToggleCtrl",
                                Toggle = new Toggle()
                                {
                                     TargetContainerId = "Toggle"
                                },
                                SectionName = "Trauma",
                                Controls = new List<Ctrl>()
                                {
                                    new TextBox() { DisplayName = "Height of Fall(ft.)"
                                       ,NgModel = "E10_10" },
                                    new DropDownList() { DisplayName = "Cause of Injury", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_01")
                                       ,NgModel = "E10_01" },
                                    new DropDownList() { DisplayName = "Intent of Injury", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_02")
                                       ,NgModel = "E10_02" },
                                    new DropDownList() { DisplayName = "Trauma Type", 
                                        DropDownOptions = options.NemsisSelectOptions("E10_03")
                                       ,NgModel = "E10_03" }
                                }
                            },
                            new Section()
                            {   PartialTemplateName = "SectionWithToggleCtrl",
                                Toggle = new Toggle()
                                {
                                     TargetContainerId = "CardiacArrest"
                                },
                                Side = SectionSideEnum.right,
                                SectionName = "Cardiac Arrest",
                                Controls = new List<Ctrl>()
                                {
                                    new TextBox() { DisplayName = "Pre-Arrival"
                                       ,NgModel = "" },
                                    new DropDownList() { DisplayName = "Cardiac Arest Time", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_08")
                                       ,NgModel = "E11_08" },
                                    new DropDownList() { DisplayName = "Etiology", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_02")
                                       ,NgModel = "E11_02" },
                                    new DropDownList() { DisplayName = "Witnessed By", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_04")
                                       ,NgModel = "E11_04" },
                                    new DropDownList() { DisplayName = "First Rhythm", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_05")
                                       ,NgModel = "E11_05" },
                                    new DropDownList() { DisplayName = "Circulation Return", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_06")
                                       ,NgModel = "E11_06" },
                                    new DropDownList() { DisplayName = "Resusitations", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_03")
                                       ,NgModel = "E11_03" },
                                    new DropDownList() { DisplayName = "Discontinue Reason", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_10")
                                       ,NgModel = "E11_10" },
                                    new TextBox() { DisplayName = "Discontinue Time"
                                       ,NgModel = "E11_09" },
                                    new TextBox() { DisplayName = "Discontinue Date"
                                       ,NgModel = "" }
                                }
                            },
                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Impression",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Complaint Location", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_11")
                                       ,NgModel = "E09_11" },
                                    new DropDownList() { DisplayName = "Organ System", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_12")
                                       ,NgModel = "E09_12" },
                                    new DropDownList() { DisplayName = "Primary Symptoms", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_13")
                                       ,NgModel = "E09_13" },
                                    new DropDownList() { DisplayName = "Other Symptoms", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_14")
                                       ,NgModel = "E09_14" },
                                    new DropDownList() { DisplayName = "Impression", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_15")
                                       ,NgModel = "E09_15" },
                                    new DropDownList() { DisplayName = "Secondary Impression", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_16")
                                       ,NgModel = "E09_16" },
                                    new DropDownList() { DisplayName = "Drugs/Alcohol", 
                                        DropDownOptions = options.NemsisSelectOptions("E12_19")
                                       ,NgModel = "E12_19" }
                                }
                            },

                            new Section()
                            {
                                PartialTemplateName = "SectionWithToggleCtrl",
                                Toggle = new Toggle()
                                {
                                     TargetContainerId = "PriorAid"
                                },
                                Side = SectionSideEnum.right,
                                SectionName = "Prior Aid Given",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Prior Aid", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_01")
                                       ,NgModel = "E09_01" },
                                    new DropDownList() { DisplayName = "Treated By", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_02")
                                       ,NgModel = "E09_02" },
                                    new DropDownList() { DisplayName = "Aid Outcome", 
                                        DropDownOptions = options.NemsisSelectOptions("E09_03")
                                       ,NgModel = "E09_03" }
                                }
                            }
                        }
                    },
                    new Tab()
                    {

                        TabTargetName = "BillingTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                SectionName = "Billing",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Condition Codes", 
                                        DropDownOptions = options.NemsisSelectOptions("E07_35")
                                       ,NgModel = "E07_35" },
                                    new DropDownList() { DisplayName = "Payment Method", 
                                        DropDownOptions = options.NemsisSelectOptions("E07_01")
                                       ,NgModel = "E07_01" },
                                    new DropDownList() { DisplayName = "Necessity Certificate", 
                                        DropDownOptions = options.NemsisSelectOptions("E07_02")
                                       ,NgModel = "E07_02" },
                                    new TextBox() { DisplayName = "Patient Email"
                                       ,NgModel = "CustomPatientEmail" }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Employer",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Work Related", 
                                        DropDownOptions = options.NemsisSelectOptions("E07_15")
                                       ,NgModel = "E07_15" },
                                    new DropDownList() { DisplayName = "Employer", 
                                        DropDownOptions = options.NemsisSelectOptions("E07_27")
                                       ,NgModel = "E07_27" },
                                    new TextBox() { DisplayName = "Employer Address"
                                       ,NgModel = "" },
                                    new TextBox() { DisplayName = "Employer Phone"
                                       ,NgModel = "E07_32" },
                                    new TextBox() { DisplayName = "Patient Occupation"
                                       ,NgModel = "E07_17" },
                                    new DropDownList() { DisplayName = "Occupation Industry", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "" }
                                }
                            }
                            ,new Section()
                            {
                                PartialTemplateName = "SectionWithToggleCtrl",
                                Toggle = new Toggle()
                                {
                                     TargetContainerId = "GuardianPatient"
                                },
                                Side = SectionSideEnum.right,
                                SectionName = "Guardian/Patient",
                                Controls = new List<Ctrl>()
                                {
                                    new TextBox() { DisplayName = "Last Name"
                                       ,NgModel = "E07_18" },
                                    new TextBox() { DisplayName = "First Name"
                                       ,NgModel = "E07_19" },
                                    new TextBox() { DisplayName = "M.I."
                                       ,NgModel = "E07_20" },
                                    new TextBox() { DisplayName = "Phone #"
                                       ,NgModel = "E07_25" },
                                    new DropDownList() { DisplayName = "Relationship", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "E07_26" },
                                    new TextBox() { DisplayName = "Guardian Address"
                                       ,NgModel = "" }
                                }
                            },
                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Insurance",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                       }
                                }
                            }
                        }
                    },        
                    new Tab()
                    {
                        PartialTemplateName = "TabSingleColumn",
                        TabTargetName = "TreatmentTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {   
                                SectionName = "Timeline",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            }
                        }
                    },
                    new Tab()
                    {
                        TabTargetName = "OutcomeTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                SectionName = "Destination/Transfer To",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Destination", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "" },
                                    new DropDownList() { DisplayName = "Destination Reason", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_16")
                                       ,NgModel = "E20_16" },
                                    new TextBox() { DisplayName = "Destination Address", ResponsiveWidth = 12
                                       ,NgModel = "" },
                                    new DropDownList() { DisplayName = "Transfer Condition", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_15")
                                       ,NgModel = "E20_15" },
                                    new DropDownList() { DisplayName = "Destination Type", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_17")
                                       ,NgModel = "E20_17" },
                                    new DropDownList() { DisplayName = "Destination Code", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_02")
                                       ,NgModel = "E20_02" }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Transport Information",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "MCI", 
                                        DropDownOptions = options.NemsisSelectOptions("E08_06")
                                       ,NgModel = "E08_06" },
                                    new DropDownList() { DisplayName = "Rythm at Destination", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_11")
                                       ,NgModel = "E11_11" },
                                    new DropDownList() { DisplayName = "Number of Patients", 
                                        DropDownOptions = options.NemsisSelectOptions("E08_05")
                                       ,NgModel = "E08_05" },
                                    new DropDownList() { DisplayName = "ER Disposition", 
                                        DropDownOptions = options.NemsisSelectOptions("E22_01")
                                       ,NgModel = "E22_01" },
                                    new DropDownList() { DisplayName = "To Ambulance Via", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_11")
                                       ,NgModel = "E20_11" },
                                    new DropDownList() { DisplayName = "Hospital Disposition", 
                                        DropDownOptions = options.NemsisSelectOptions("E22_02")
                                       ,NgModel = "E22_02" },
                                    new DropDownList() { DisplayName = "Transport Position", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_12")
                                       ,NgModel = "E20_12" },
                                    new DropDownList() { DisplayName = "From Ambulance Via", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_13")
                                       ,NgModel = "E20_13" },
                                    new DropDownList() { DisplayName = "Mode From Scene", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_14")
                                       ,NgModel = "E20_14" }
                                }
                            },

                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Other Reporting Info",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Discharge Neuro", 
                                        DropDownOptions = options.NemsisSelectOptions("E11_07")
                                       ,NgModel = "E11_07" },
                                    new TextBox() { DisplayName = "Transfer-To Record #"
                                       ,NgModel = "E12_02" },
                                    new TextBox() { DisplayName = "Trauma Registry ID"
                                       ,NgModel = "E22_04" },
                                    new TextBox() { DisplayName = "Destination Record #"
                                       ,NgModel = "E12_03" },
                                    new TextBox() { DisplayName = "Fire Report #"
                                       ,NgModel = "E22_05" },
                                    new DropDownList() { DisplayName = "Destination Zone", 
                                        DropDownOptions = options.NemsisSelectOptions("E20_09")
                                       ,NgModel = "E20_09" },
                                    new TextBox() { DisplayName = "Patient ID Tag #"
                                       ,NgModel = "E22_06" },
                                    new TextBox() { DisplayName = "Vehicle Lat GPS"
                                       ,NgModel = "E02_15" },
                                    new TextBox() { DisplayName = "Destination Lat GPS"
                                       ,NgModel = "E20_08" },
                                    new TextBox() { DisplayName = "Vehicle Long GPS"
                                       ,NgModel = "E02_15" },
                                    new TextBox() { DisplayName = "Destination Long GPS"
                                       ,NgModel = "E20_08" }
                                }
                            }
                        }
                    },
                    new Tab()
                    {
                        PartialTemplateName = "TabSingleColumn",
                        TabTargetName = "NarrativeTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                
                                SectionName = "Narrative",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                ResponsiveWidth = 6,
                                SectionName = "Other Fields",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Review Requested", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_01")
                                       ,NgModel = "E23_01" },
                                    new DropDownList() { DisplayName = "EMS Injury", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_05")
                                       ,NgModel = "E23_05" },
                                    new DropDownList() { DisplayName = "Injury Type", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_05")
                                       ,NgModel = "E23_06" },
                                    new DropDownList() { DisplayName = "Contact Blood/Fluids", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_08")
                                       ,NgModel = "E23_05" },
                                    new DropDownList() { DisplayName = "Fluid Exposure Type", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_06")
                                       ,NgModel = "E23_06" },
                                    new DropDownList() { DisplayName = "Personnel Exposed", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_07")
                                       ,NgModel = "E23_07" },
                                    new DropDownList() { DisplayName = "Req. Reportable Cond.", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_08")
                                       ,NgModel = "E23_08" },
                                    new DropDownList() { DisplayName = "Registry Candidate", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_02")
                                       ,NgModel = "E23_02" },
                                    new DropDownList() { DisplayName = "Protective Equipment", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_03")
                                       ,NgModel = "E23_03" },
                                    new DropDownList() { DisplayName = "Disasters", 
                                        DropDownOptions = options.NemsisSelectOptions("E23_04")
                                       ,NgModel = "E23_04" },
                                    new DropDownList() { DisplayName = "Precautions", 
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "" }
                                }
                            }
                        }
                    },
                    new Tab()
                    {
                        PartialTemplateName = "TabSingleColumn",
                        TabTargetName = "DocumentsTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                SectionName = "Forms",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Attachments",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            }

                        }
                    },
                    new Tab()
                    {
                        PartialTemplateName = "TabSingleColumn",
                        TabTargetName = "SignaturesTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                SectionName = "Signatures",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            }
                        }
                    },
                    new Tab()
                    {
                        PartialTemplateName = "TabSingleColumn",
                        TabTargetName = "NotesTab",
                        Sections = new List<Section>()
                        {
                            new Section()
                            {
                                SectionName = "Notes",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            }
                        }
                    }
                }
            };

            //Tab tab = new Tab();

            //Section section = new Section() { SectionName = "Test 1", ResponsiveWidth = 9 };

            //Ctrl control = new Ctrl() { DisplayName = "FirstName", ControlType = ControlTypeEnum.Select2,
            //    DropDownOptions = GetSelect2Options("E1_01", agencyToken, state, select2OptionsLists)
            //};



            //section.Controls.Add(control);

            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);

            //tab.Sections.Add(section);


            //tab.Sections.Add(section);
            //tab.Sections.Add(section);

            //pcrForm.Tabs.Add(tab);

            //tab = new Tab();

            //section = new Section() { SectionName = "Raggedy Ann" }; ;

            //control = new Ctrl() { DisplayName = "DropDown", ControlType = ControlTypeEnum.DropDownList,
            //DropDownOptions = CtrlHelpers.GetSelect2Options(new string[] { "one", "two", "three" })
            //};

            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);
            //section.Controls.Add(control);

            //tab.Sections.Add(section);
            //tab.Sections.Add(section);
            //tab.Sections.Add(section);

            //pcrForm.Tabs.Add(tab);



            return View(pcrForm);
        }



        // GET: Mpa/PcrForm/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/PcrForm/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/PcrForm/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Mpa/PcrForm/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/PcrForm/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Mpa/PcrForm/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/PcrForm/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
