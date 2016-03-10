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
                                            new DropDownList() { DisplayName = "Disposition/Outcome2", ControlType = ControlTypeEnum.DropDownList,
                                                DropDownOptions = options.NemsisSelectOptions("E20_10"), ResponsiveWidth = 12
                                                }

                                        },
                                         OnCancelClick = "alert('cancel')",
                                         OnSubmitClick = "alert('submit')"
                                     
                                },

                                SectionName = "Disposition",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Disposition/Outcome", ControlType = ControlTypeEnum.DropDownList,
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
                                   new Ctrl() { DisplayName = "Incident Number", ControlType = ControlTypeEnum.TextBox
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
                                    new DropDownList() { DisplayName = "Type Of Location", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E08_07")
                                        ,NgModel = "E08_07"
                                        },
                                    new DropDownList() { DisplayName = "Nature Of Incident", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E03_01")
                                        ,NgModel = "E03_01"
                                        },
                                    new Ctrl() { DisplayName = "Scene Address", ControlType = ControlTypeEnum.TextBox
                                        ,NgModel = ""
                                        }

                                }
                            },
                            new Section()
                            {    SectionName = "Dispatch",
                                Controls = new List<Ctrl>()
                                {
                                   new DropDownList() { DisplayName = "Call Sign", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E02_12")
                                        ,NgModel = "E02_12"
                                        },
                                        new DropDownList() { DisplayName = "Vehicle Number", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E02_11")
                                        ,NgModel = "E02_11"
                                        },
                                    new DropDownList() { DisplayName = "Mode To Scene", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E02_20")
                                        ,NgModel = "E02_20"
                                        },
                                        new Ctrl() { DisplayName = "Veh. Incident #", ControlType = ControlTypeEnum.TextBox
                                        ,NgModel = ""
                                        },
                                    new DropDownList() { DisplayName = "Service Requested", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E02_04")
                                        ,NgModel = "E02_04"
                                        },
                                    new DropDownList() { DisplayName = "Role", ControlType = ControlTypeEnum.DropDownList,
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
                                    new DropDownList() { DisplayName = "Services On Scene", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E08_02")
                                        ,NgModel = "E08_02"
                                        },
                                    new DropDownList() { DisplayName = "Other EMS Agencies", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E08_01")
                                        ,NgModel = "E08_01"
                                        },
                                    new Ctrl() { DisplayName = "EMS System", ControlType = ControlTypeEnum.TextBox
                                        ,NgModel = "D04_17"
                                        },
                                    new Ctrl() { DisplayName = "Response Differential", ControlType = ControlTypeEnum.TextBox
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
                                   new Ctrl() { DisplayName = "Incident Date", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                        ,NgModel = ""
                                   },
                                   new Ctrl() { DisplayName = "Onset", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                   ,NgModel = "E05_01"
                                   },
                                    new Ctrl() { DisplayName = "Recieved", ControlType = ControlTypeEnum.TimePicker,  ResponsiveWidth = 12
                                        ,NgModel = "E05_02"},
                                   new Ctrl() { DisplayName = "Notified", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                        ,NgModel = "E05_03"},
                                    new Ctrl() { DisplayName = "Dispatched", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_04"},
                                     new Ctrl() { DisplayName = "Enroute", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                        ,NgModel = "E05_05"},
                                    new Ctrl() { DisplayName = "Arrival", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_06"},
                                    new Ctrl() { DisplayName = "Contacted", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_07"},
                                    new Ctrl() { DisplayName = "Transfer", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_08"},
                                    new Ctrl() { DisplayName = "Departed", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_09"},
                                    new Ctrl() { DisplayName = "Arrival", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_10"},
                                    new Ctrl() { DisplayName = "Available", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_11"},
                                    new Ctrl() { DisplayName = "At Base", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        ,NgModel = "E05_13"},
                                    new Ctrl() { DisplayName = "Cancelled", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
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
                                   new DropDownList() { DisplayName = "Primary", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = "E04_02"},
                                    new DropDownList() { DisplayName = "Secondary", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = ""},
                                    new DropDownList() { DisplayName = "Third", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions(""), ResponsiveWidth = 12
                                        ,NgModel = ""},
                                    new DropDownList() { DisplayName = "Other", ControlType = ControlTypeEnum.DropDownList,
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
                                   new Ctrl() { DisplayName = "Start", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        ,NgModel = "E02_16"},
                                    new Ctrl() { DisplayName = "Scene", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        ,NgModel = "E02_17"},
                                    new Ctrl() { DisplayName = "Dest.", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        ,NgModel = "E02_18"},
                                    new Ctrl() { DisplayName = "Service", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
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
                                    new Ctrl() { DisplayName = "First Name", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_02"},
                                    new Ctrl() { DisplayName = "Phone", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_17"},
                                    new Ctrl() { DisplayName = "Last Name", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_01"},
                                    new Ctrl() { DisplayName = "Weight", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E16_01"},
                                    new Ctrl() { DisplayName = "M.I.", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_03"},
                                    new DropDownList() { DisplayName = "Race", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E06_12")
                                       ,NgModel = "E06_12"},
                                    new Ctrl() { DisplayName = "DOB", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_16"},
                                    new DropDownList() { DisplayName = "Ethnicity", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E06_13")
                                       ,NgModel = "E06_13"},
                                    new DropDownList() { DisplayName = "Gender", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E06_11")
                                       ,NgModel = "E06_11"},
                                    new Ctrl() { DisplayName = "SSN", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_10"},
                                    new Ctrl() { DisplayName = "Pattient Address", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = ""}
                                    }
                             },
                             new Section()
                             {
                                 SectionName = "Personal",
                                 Controls = new List<Ctrl>()
                                 {

                                    new Ctrl() { DisplayName = "DL Number", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_19"},
                                    new Ctrl() { DisplayName = "Pt Practitioner Name", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E06_18"},
                                     new DropDownList() { DisplayName = "DL State", ControlType = ControlTypeEnum.DropDownList,
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
                                     new DropDownList() { DisplayName = "History", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_10")
                                       ,NgModel = "E12_10"},
                                    new DropDownList() { DisplayName = "History Obtained", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_11")
                                       ,NgModel = "E12_11"},
                                    new DropDownList() { DisplayName = "Allergies (Meds)", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_08")
                                       ,NgModel = "E12_08"},
                                    new DropDownList() { DisplayName = "Emergency Form", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_18")
                                       ,NgModel = "E12_18" },
                                    new DropDownList() { DisplayName = "Allergies (Other)", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_09")
                                       ,NgModel = "E12_09" },
                                    new DropDownList() { DisplayName = "Advanced Directives", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_07")
                                       ,NgModel = "E12_07" },
                                    new DropDownList() { DisplayName = "Triage Color", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "CustomTriageColor" },
                                    new DropDownList() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "CustomTriageCategory" },
                                    new DropDownList() { DisplayName = "Pregnant?", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_20")
                                       ,NgModel = "E12_20" },
                                    new DropDownList() { DisplayName = "# Past Pregnancies", ControlType = ControlTypeEnum.DropDownList,
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
                                    new Ctrl() { DisplayName = "Chief Complaint", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E09_05" },
                                    new Ctrl() { DisplayName = "Duration", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 3
                                       ,NgModel = "E09_06" },
                                    new DropDownList() { DisplayName = "Units", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_07"), ResponsiveWidth = 3
                                       ,NgModel = "E09_07" },
                                    new Ctrl() { DisplayName = "Secondary Complaint", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E09_08" },
                                    new Ctrl() { DisplayName = "Duration", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 3
                                       ,NgModel = "E09_09" },
                                    new DropDownList() { DisplayName = "Units", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_10"), ResponsiveWidth = 3
                                       ,NgModel = "E09_10" },
                                    new DropDownList() { DisplayName = "Barriers To Patient Care", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E12_01"), ResponsiveWidth = 12
                                       ,NgModel = "E12_01" }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Exams",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Patient Complaints", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12 }

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
                                    new DropDownList() { DisplayName = "Vehicle Collision Impact", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_05"), ResponsiveWidth = 12
                                       ,NgModel = "E10_05" },
                                    new Ctrl() { DisplayName = "Report Number", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E22_03" },
                                    new DropDownList() { DisplayName = "Pt Location", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_07")
                                       ,NgModel = "E10_07" },
                                    new DropDownList() { DisplayName = "Row", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_06")
                                       ,NgModel = "E10_06" },
                                    new DropDownList() { DisplayName = "Safety Equipment", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_08")
                                       ,NgModel = "E10_08" },
                                    new DropDownList() { DisplayName = "Airbags", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_09")
                                       ,NgModel = "E10_09" },
                                    new DropDownList() { DisplayName = "Injury Indicators", ControlType = ControlTypeEnum.DropDownList,
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
                                    new Ctrl() { DisplayName = "Height of Fall(ft.)", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E10_10" },
                                    new DropDownList() { DisplayName = "Cause of Injury", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_01")
                                       ,NgModel = "E10_01" },
                                    new DropDownList() { DisplayName = "Intent of Injury", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E10_02")
                                       ,NgModel = "E10_02" },
                                    new DropDownList() { DisplayName = "Trauma Type", ControlType = ControlTypeEnum.DropDownList,
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
                                    new Ctrl() { DisplayName = "Pre-Arrival", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "" },
                                    new DropDownList() { DisplayName = "Cardiac Arest Time", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_08")
                                       ,NgModel = "E11_08" },
                                    new DropDownList() { DisplayName = "Etiology", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_02")
                                       ,NgModel = "E11_02" },
                                    new DropDownList() { DisplayName = "Witnessed By", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_04")
                                       ,NgModel = "E11_04" },
                                    new DropDownList() { DisplayName = "First Rhythm", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_05")
                                       ,NgModel = "E11_05" },
                                    new DropDownList() { DisplayName = "Circulation Return", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_06")
                                       ,NgModel = "E11_06" },
                                    new DropDownList() { DisplayName = "Resusitations", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_03")
                                       ,NgModel = "E11_03" },
                                    new DropDownList() { DisplayName = "Discontinue Reason", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_10")
                                       ,NgModel = "E11_10" },
                                    new Ctrl() { DisplayName = "Discontinue Time", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E11_09" },
                                    new Ctrl() { DisplayName = "Discontinue Date", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "" }
                                }
                            },
                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Impression",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Complaint Location", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_11")
                                       ,NgModel = "E09_11" },
                                    new DropDownList() { DisplayName = "Organ System", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_12")
                                       ,NgModel = "E09_12" },
                                    new DropDownList() { DisplayName = "Primary Symptoms", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_13")
                                       ,NgModel = "E09_13" },
                                    new DropDownList() { DisplayName = "Other Symptoms", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_14")
                                       ,NgModel = "E09_14" },
                                    new DropDownList() { DisplayName = "Impression", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_15")
                                       ,NgModel = "E09_15" },
                                    new DropDownList() { DisplayName = "Secondary Impression", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_16")
                                       ,NgModel = "E09_16" },
                                    new DropDownList() { DisplayName = "Drugs/Alcohol", ControlType = ControlTypeEnum.DropDownList,
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
                                    new DropDownList() { DisplayName = "Prior Aid", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_01")
                                       ,NgModel = "E09_01" },
                                    new DropDownList() { DisplayName = "Treated By", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E09_02")
                                       ,NgModel = "E09_02" },
                                    new DropDownList() { DisplayName = "Aid Outcome", ControlType = ControlTypeEnum.DropDownList,
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
                                    new DropDownList() { DisplayName = "Condition Codes", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E07_35")
                                       ,NgModel = "E07_35" },
                                    new DropDownList() { DisplayName = "Payment Method", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E07_01")
                                       ,NgModel = "E07_01" },
                                    new DropDownList() { DisplayName = "Necessity Certificate", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E07_02")
                                       ,NgModel = "E07_02" },
                                    new DropDownList() { DisplayName = "Patient Email", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "CustomPatientEmail" }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Employer",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "Work Related", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E07_15")
                                       ,NgModel = "E07_15" },
                                    new DropDownList() { DisplayName = "Employer", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E07_27")
                                       ,NgModel = "E07_27" },
                                    new Ctrl() { DisplayName = "Employer Address", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "" },
                                    new Ctrl() { DisplayName = "Employer Phone", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E07_32" },
                                    new Ctrl() { DisplayName = "Patient Occupation", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E07_17" },
                                    new DropDownList() { DisplayName = "Occupation Industry", ControlType = ControlTypeEnum.DropDownList,
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
                                    new Ctrl() { DisplayName = "Last Name", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E07_18" },
                                    new Ctrl() { DisplayName = "First Name", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E07_19" },
                                    new Ctrl() { DisplayName = "M.I.", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E07_20" },
                                    new Ctrl() { DisplayName = "Phone #", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E07_25" },
                                    new DropDownList() { DisplayName = "Relationship", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "E07_26" },
                                    new Ctrl() { DisplayName = "Guardian Address", ControlType = ControlTypeEnum.TextBox
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
                                    new DropDownList() { DisplayName = "Destination", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "" },
                                    new DropDownList() { DisplayName = "Destination Reason", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_16")
                                       ,NgModel = "E20_16" },
                                    new Ctrl() { DisplayName = "Destination Address", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                       ,NgModel = "" },
                                    new DropDownList() { DisplayName = "Transfer Condition", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_15")
                                       ,NgModel = "E20_15" },
                                    new DropDownList() { DisplayName = "Destination Type", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_17")
                                       ,NgModel = "E20_17" },
                                    new DropDownList() { DisplayName = "Destination Code", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_02")
                                       ,NgModel = "E20_02" }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Transport Information",
                                Controls = new List<Ctrl>()
                                {
                                    new DropDownList() { DisplayName = "MCI", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E08_06")
                                       ,NgModel = "E08_06" },
                                    new DropDownList() { DisplayName = "Rythm at Destination", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_11")
                                       ,NgModel = "E11_11" },
                                    new DropDownList() { DisplayName = "Number of Patients", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E08_05")
                                       ,NgModel = "E08_05" },
                                    new DropDownList() { DisplayName = "ER Disposition", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E22_01")
                                       ,NgModel = "E22_01" },
                                    new DropDownList() { DisplayName = "To Ambulance Via", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_11")
                                       ,NgModel = "E20_11" },
                                    new DropDownList() { DisplayName = "Hospital Disposition", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E22_02")
                                       ,NgModel = "E22_02" },
                                    new DropDownList() { DisplayName = "Transport Position", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_12")
                                       ,NgModel = "E20_12" },
                                    new DropDownList() { DisplayName = "From Ambulance Via", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_13")
                                       ,NgModel = "E20_13" },
                                    new DropDownList() { DisplayName = "Mode From Scene", ControlType = ControlTypeEnum.DropDownList,
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
                                    new DropDownList() { DisplayName = "Discharge Neuro", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E11_07")
                                       ,NgModel = "E11_07" },
                                    new Ctrl() { DisplayName = "Transfer-To Record #", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E12_02" },
                                    new DropDownList() { DisplayName = "Trauma Registry ID", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E22_04" },
                                    new Ctrl() { DisplayName = "Destination Record #", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E12_03" },
                                    new Ctrl() { DisplayName = "Fire Report #", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E22_05" },
                                    new DropDownList() { DisplayName = "Destination Zone", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E20_09")
                                       ,NgModel = "E20_09" },
                                    new Ctrl() { DisplayName = "Patient ID Tag #", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E22_06" },
                                    new Ctrl() { DisplayName = "Vehicle Lat GPS", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E02_15" },
                                    new Ctrl() { DisplayName = "Destination Lat GPS", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E20_08" },
                                    new Ctrl() { DisplayName = "Vehicle Long GPS", ControlType = ControlTypeEnum.TextBox
                                       ,NgModel = "E02_15" },
                                    new Ctrl() { DisplayName = "Destination Long GPS", ControlType = ControlTypeEnum.TextBox
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
                                    new DropDownList() { DisplayName = "Review Requested", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_01")
                                       ,NgModel = "E23_01" },
                                    new DropDownList() { DisplayName = "EMS Injury", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_05")
                                       ,NgModel = "E23_05" },
                                    new DropDownList() { DisplayName = "Injury Type", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_05")
                                       ,NgModel = "E23_06" },
                                    new DropDownList() { DisplayName = "Contact Blood/Fluids", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_08")
                                       ,NgModel = "E23_05" },
                                    new DropDownList() { DisplayName = "Fluid Exposure Type", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_06")
                                       ,NgModel = "E23_06" },
                                    new DropDownList() { DisplayName = "Personnel Exposed", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_07")
                                       ,NgModel = "E23_07" },
                                    new DropDownList() { DisplayName = "Req. Reportable Cond.", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_08")
                                       ,NgModel = "E23_08" },
                                    new DropDownList() { DisplayName = "Registry Candidate", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_02")
                                       ,NgModel = "E23_02" },
                                    new DropDownList() { DisplayName = "Protective Equipment", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_03")
                                       ,NgModel = "E23_03" },
                                    new DropDownList() { DisplayName = "Disasters", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = options.NemsisSelectOptions("E23_04")
                                       ,NgModel = "E23_04" },
                                    new DropDownList() { DisplayName = "Precautions", ControlType = ControlTypeEnum.DropDownList,
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
