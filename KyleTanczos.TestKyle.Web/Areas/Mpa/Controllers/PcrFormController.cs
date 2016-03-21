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

    public class SectionTimeLine : Section
    {
        public Dialog VitalsDialog { get; set; }
        public Dialog MedicationsDialog { get; set; }
        public Dialog ProceduresDialog { get; set; }
        public DialogExams ExamsDialog { get; set; }

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


    public class DialogAddress : Dialog
    {
        public DialogAddress()
        {
            PartialTemplateName = "DialogAddress";
            SubmitBtnText = "Close";
            HideCancelButton = true;
            //OnSubmitClick = "alert('hello');"; //SetTextArea('TextAreaSceneAddress', this);           
        }

        public new string OnSubmitClick { get { return "alert('hello2');"; } } //SetTextArea('TextAreaSceneAddress', this); 

        public string AddressName { get; set; }

        public new List<Ctrl> Controls { get { return
                    new List<Ctrl>()
                    {
                        new TextBox() { DisplayName = "Google Address (Quick Search)", NgModel= AddressName + "Address.AutoComplete", ResponsiveWidth = 12
                            , CustomAttributes = "ng-autocomplete details=details"
                            //, DropDownOptions = new List<Select2Option>() { new Select2Option() { id = "Herm", text = "Hermitage" }, new Select2Option() { id = "sharon", text = "Sharon" }, new Select2Option() { id = "pitts", text = "Pittsburgh" } }
                            },
                        new TextBox() { DisplayName = "Street Address", NgModel= AddressName + "Address.Street", ResponsiveWidth = 12
                            },
                        new TextBox() { DisplayName = "Street Address 2", NgModel= AddressName + "Address.Street2", ResponsiveWidth = 12
                            },
                        new TextBox() { DisplayName = "City", NgModel= AddressName + "Address.City", ResponsiveWidth = 7
                            },
                        new TextBox() { DisplayName = "State", NgModel= AddressName + "Address.State", ResponsiveWidth = 2
                            },
                        new TextBox() { DisplayName = "Zip", NgModel= AddressName + "Address.Zip", ResponsiveWidth = 3
                            },
                        new DropDownList() { DisplayName = "Municipality Picker", NgModel= AddressName + "Address.FipsPicker", ResponsiveWidth = 12, select2_OfflineListName = "PatientMedicationList"
                                , DropDownOptions = new List<Select2Option>() { new Select2Option() { id = "Herm", text = "Hermitage" }, new Select2Option() { id = "sharon", text = "Sharon" }, new Select2Option() { id = "pitts", text = "Pittsburgh" } }
                            },
                        new TextBox() { DisplayName = "Municipality Code", NgModel= AddressName + "Address.MunicipalityCode", ResponsiveWidth = 6
                            },
                        new TextBox() { DisplayName = "County Code", NgModel= AddressName + "Address.CountyCode", ResponsiveWidth = 6
                            },
                        new TextBox() { DisplayName = "Additional Notes", NgModel= AddressName + "Address.AddressNotes", ResponsiveWidth = 12
                            }



                    };
            } }

    }

    public class DialogExams : Dialog
    {
        public List<Ctrl> Tab1Controls { get; set; }
        public List<Ctrl> Tab2Controls { get; set; }
        public List<Ctrl> Tab3Controls { get; set; }
        public List<Ctrl> Tab4Controls { get; set; }
        public List<Ctrl> Tab5Controls { get; set; }

    }


    public class Dialog
    {
        public Dialog()
        {
            PartialTemplateName = "Dialog";
            Controls = new List<Ctrl>();
            DialogTitle = "Title Here";
            SubmitBtnText = "Save";
            CancelBtnText = "Cancel";
            AllowKeepOpen = false;
        }

        public string PartialTemplateName { get; set; }
        public string DialogTargetId { get; set; }
        public string NgFormName { get; set; }
        public string DialogTitle { get; set; }
        public List<Ctrl> Controls { get; set; }
        public string OnSubmitClick { get; set; }
        public string OnCancelClick { get; set; }
        public string NgSubmitClick { get; set; }
        public string NgCancelClick { get; set; }
        public string SubmitBtnText { get; set; }
        public string CancelBtnText { get; set; }
        public bool HideCancelButton { get; set; }
        public string GetHideBtnClass { get { return (HideCancelButton ? "hidden" : ""); } }

        public bool AllowKeepOpen { get; set; }
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
            return (IsSelect2 ? "select2Default" : "");
        }
        public bool IsSelect2modal { get; set; }

        public string GetSelect2ModalClass()
        {
            return (IsSelect2modal ? "select2modal" : "");
        }

        public string select2_OfflineListName { get; set; }

        public string RenderSelect2_OfflineList()
        {
            return (string.IsNullOrEmpty(select2_OfflineListName) ? "" : "offlineSelect2listName=" + select2_OfflineListName);
        }




    }

    public class TimePicker : Ctrl
    {
        public TimePicker()
        {
            ControlType = ControlTypeEnum.TimePicker;
        }
    }

    public class AddressPicker : Ctrl
    {
        public AddressPicker()
        {
            ControlType = ControlTypeEnum.AddressPicker;

        }


        public Dialog Dialog { get; set; }


    }

    public class TableListView : Ctrl
    {
        public TableListView()
        {
            ControlType = ControlTypeEnum.TableListView;
            DisplayNames = new List<string>();
        }

        public string ngListName { get; set; }
        public List<string> ngFieldNames { get; set; }
        public List<string> DisplayNames { get; set; }
        public string NgFormName { get; set; }

}


    public class TextBox : Ctrl
    {
        public TextBox()
        {
            ControlType = ControlTypeEnum.TextBox;
        }
    }

    public class Select2 : Ctrl
    {
        public Select2()
        {
            ControlType = ControlTypeEnum.Select2;
        }


    }

    public class TextArea : Ctrl
    {
        public TextArea()
        {
            ControlType = ControlTypeEnum.TextArea;
        }

        public int HeightRows { get; set; }
    }

    public class Ctrl
    {
        public Ctrl()
        {
            ResponsiveWidth = 6;
            DialogShowOnlyOnMore = false; 
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

        public string CustomAttributes { get; set; }

        public string ContainerCustomCssClass { get; set; }
        public string ControlCustomCssClass { get; set; }
        public bool DialogShowOnlyOnMore { get; set; }

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

                                 
    public enum ControlTypeEnum { PatientMeds, MileageBox, TextBox, DropDownList, Select2, Select2Single, TextArea, Select2Many, AddressPicker, Select2TagsSingle, Select2TagsMany, TableListView, TimePicker }

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

    //[OutputCache(Duration = 120, VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]
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
                                        ,NgModel = "E20_10", IsSelect2 = true
                                        }
                                }
                            },
                            new Section()
                            {

                                SectionName = "Incident",
                                Controls = new List<Ctrl>()
                                {
                                   new TextBox() { DisplayName = "Incident Number"
                                       ,NgModel = "E02_02", ControlCustomCssClass = "input-spinner"
                                        },
                                    new Select2() { DisplayName = "Response Urgency"
                                        ,NgModel = "E07_33"
                                        }
                                    ,
                                    new AddressPicker() { DisplayName = "Scene Address"
                                        , Dialog = new DialogAddress()
                                            {
                                                DialogTargetId = "SceneAddress",

                                                AddressName = "Scene"
                                            }

                                        },
                                    new DropDownList() { DisplayName = "CMS Level",
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
                                   new TimePicker() { DisplayName = "Onset",  ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                   ,NgModel = "E05_01"
                                   },
                                    new TimePicker() { DisplayName = "Recieved",   ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_02"},
                                   new TimePicker() { DisplayName = "Notified",  ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_03"},
                                    new TimePicker() { DisplayName = "Dispatched", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_04"},
                                     new TimePicker() { DisplayName = "Enroute",  ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_05"},
                                    new TimePicker() { DisplayName = "Arrival", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_06"},
                                    new TimePicker() { DisplayName = "Contacted", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_07"},
                                    new TimePicker() { DisplayName = "Transfer", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_08"},
                                    new TimePicker() { DisplayName = "Departed", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_09"},
                                    new TimePicker() { DisplayName = "Arrival", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_10"},
                                    new TimePicker() { DisplayName = "Available", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_11"},
                                    new TimePicker() { DisplayName = "At Base", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
                                        ,NgModel = "E05_13"},
                                    new TimePicker() { DisplayName = "Cancelled", ResponsiveWidth = 12, PlaceHolder = "xx:xx"
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
                                        DropDownOptions = options.NemsisSelectOptions("E04_02"), ResponsiveWidth = 12
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
                                    new TextBox() { DisplayName = "Patient Address"
                                       ,NgModel = "E06_04_0"}
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
                                       ,NgModel = "E12_06"},
                                     new DropDownList() { DisplayName = "DL State",
                                        DropDownOptions = options.NemsisSelectOptions("E06_18")
                                       ,NgModel = "E06_18"}
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
                                       ,NgModel = "E12_20" }/*,
                                    new DropDownList() { DisplayName = "# Past Pregnancies",
                                        DropDownOptions = options.NemsisSelectOptions("")
                                       ,NgModel = "" }*/
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

                             },
                            new Section()
                            {
                                PartialTemplateName = "SectionWithDialog",

                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "Immunizations",
                                        DialogTitle = "Immunizations",
                                        NgFormName = "Immunizations",
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Immunizations.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new DropDownList() { DisplayName = "Immunization Date", NgModel = "forms.Immunizations.E12_13",
                                                DropDownOptions = options.NemsisSelectOptions("E12_13"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Immunization Type", NgModel = "forms.Immunizations.E12_13",
                                                DropDownOptions = options.NemsisSelectOptions("E12_13"), ResponsiveWidth = 12
                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                            NgSubmitClick = "AddItemToList('Immunizations');",
                                            NgCancelClick = "ClearCloseModal('#Immunizations', 'Immunizations');"
                                },

                                SectionName = "Immunizations",
                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "Immunizations",
                                        ngFieldNames =  new List<string>() { "ImmunDate", "ImmunType" },
                                        DisplayNames = new List<string>() { "Date", "Type" }
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
                                    new DropDownList() { DisplayName = "Units", IsSelect2 = true,
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
                                       ,NgModel = "E07_28-E07_31" },
                                    new TextBox() { DisplayName = "Employer Phone"
                                       ,NgModel = "E07_32" },
                                    new TextBox() { DisplayName = "Patient Occupation"
                                       ,NgModel = "E07_17" },
                                    new DropDownList() { DisplayName = "Occupation Industry",
                                        DropDownOptions = options.NemsisSelectOptions("E07_16")
                                       ,NgModel = "E07_16" }
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
                                       ,NgModel = "E07_21" }
                                }
                            },
                            new Section()
                            {
                                PartialTemplateName = "SectionWithDialog",
                                SectionName = "Insurances",
                                Side = SectionSideEnum.right,

                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "Insurances",
                                        DialogTitle = "Insurances",
                                        NgFormName = "Insurances",
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Insurances.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Insurance", NgModel = "forms.Insurances.E07_03"
                                                },
                                            new TextBox() { DisplayName = "Billing Priority", NgModel = "forms.Insurances.E07_04"
                                                },
                                            new TextBox() { DisplayName = "Group", NgModel = "forms.Insurances.E07_09"
                                                },
                                            new TextBox() { DisplayName = "Policy", NgModel = "forms.Insurances.E07_10"
                                                },
                                            new TextBox() { DisplayName = "Insurance Different Than Patient", NgModel = "InsuranceForm.insuranceDifferentThanPatientSelect"
                                                },
                                            new TextBox() { DisplayName = "Primary Last Name", NgModel = "forms.Insurances.E07_11"
                                                },
                                            new TextBox() { DisplayName = "Primary First Name", NgModel = "forms.Insurances.E07_12"
                                                },
                                            new TextBox() { DisplayName = "Primary Middle Name", NgModel = "forms.Insurances.E07_13"
                                                },
                                            new DropDownList() { DisplayName = "Relationship To Patient", NgModel = "forms.Insurances.E07_14"
                                                },
                                            new DropDownList() { DisplayName = "Street", NgModel = "forms.Insurances.E07_05"
                                                },
                                            new DropDownList() { DisplayName = "City", NgModel = "forms.Insurances.E07_06", ResponsiveWidth = 6
                                                },
                                            new DropDownList() { DisplayName = "State", NgModel = "forms.Insurances.E07_07", ResponsiveWidth = 3
                                                },
                                            new DropDownList() { DisplayName = "Zip", NgModel = "forms.Insurances.E07_08", ResponsiveWidth = 3
                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                            NgSubmitClick = "AddItemToList('Insurances');",
                                            NgCancelClick = "ClearCloseModal('#Insurances', 'Insurances');"
                                },

                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "Insurances",
                                        ngFieldNames =  new List<string>() { "InsuranceName", "Group", "Policy" },
                                        DisplayNames = new List<string>() { "Insurance", "Group", "Policy" }
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
                            new SectionTimeLine()
                            {
                                PartialTemplateName = "SectionTimeline",
                                SectionName = "Timeline",

                                VitalsDialog = new Dialog()
                                {
                                        DialogTargetId = "Vitals",
                                        DialogTitle = "Vitals",
                                        NgFormName = "Vitals",
                                         AllowKeepOpen = true,
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Vitals.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Date", NgModel = "forms.Vitals.V0a"
                                                },
                                            new TextBox() { DisplayName = "Time", NgModel = "forms.Vitals.V0b"
                                                },
                                            new TextBox() { DisplayName = "SBP", NgModel = "forms.Vitals.E14_04", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "DBP", NgModel = "forms.Vitals.E14_05", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "BP Device", NgModel = "forms.Vitals.E14_06", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "AVPU", NgModel = "forms.Vitals.E14_22", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "Respiration  ", NgModel = "forms.Vitals.E14_11", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "Pulse Ox", NgModel = "forms.Vitals.E14_09", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "Pulse Rythm", NgModel = "forms.Vitals.E14_10", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "Respiration Effort", NgModel = "forms.Vitals.E14_12", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "Pulse Rate", NgModel = "forms.Vitals.E14_07", ResponsiveWidth = 4
                                                },
                                            new TextBox() { DisplayName = "Temp(F)", NgModel = "forms.Vitals.E14_20", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Temp Method", NgModel = "forms.Vitals.E14_21", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Pain", NgModel = "forms.Vitals.V0d", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Heart Rate", NgModel = "forms.Vitals.V0e", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Cardiac Rhythm", NgModel = "forms.Vitals.E14_03", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "CO2", NgModel = "forms.Vitals.E14_13", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Blood Glucose", NgModel = "forms.Vitals.E14_14", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Thrombolytic", NgModel = "forms.Vitals.E14_25", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Stroke Scale", NgModel = "forms.Vitals.E14_24", ResponsiveWidth = 6, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "APGAR", NgModel = "forms.Vitals.E14_26", ResponsiveWidth = 6, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "GCS Eyes", NgModel = "forms.Vitals.E14_15", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "GCS Verbal", NgModel = "forms.Vitals.E14_16", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "GCS Motor", NgModel = "forms.Vitals.E14_17", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "GCS Qaul.", NgModel = "forms.Vitals.E14_18", ResponsiveWidth = 6, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "GCS Total", NgModel = "forms.Vitals.E14_19", ResponsiveWidth = 6, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Revised Trauma", NgModel = "forms.Vitals.E14_27", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Pediatric Trauma", NgModel = "forms.Vitals.E14_28", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                },
                                            new TextBox() { DisplayName = "Is Prior Aid", NgModel = "forms.Vitals.E14_02", ResponsiveWidth = 4, DialogShowOnlyOnMore = true
                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                            NgSubmitClick = "AddItemToList('Vitals');",
                                            NgCancelClick = "ClearCloseModal('#Vitals', 'Vitals');"
                                },

                                MedicationsDialog = new Dialog()
                                {
                                        DialogTargetId = "Medications",
                                        DialogTitle = "Medications",
                                        NgFormName = "Medications",
                                         AllowKeepOpen = true,
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Medications.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Time", NgModel = "forms.Medications.E18_01"
                                                },
                                            new TextBox() { DisplayName = "Date", NgModel = "forms.Medications.E18_01"
                                                },
                                            new TextBox() { DisplayName = "Crew", NgModel = "forms.Medications.E04_01"
                                                },
                                            new TextBox() { DisplayName = "Medication", NgModel = "forms.Medications.E18_03"
                                                },
                                            new TextBox() { DisplayName = "Dosage", NgModel = "forms.Medications.E18_05"
                                                },
                                            new TextBox() { DisplayName = "Units", NgModel = "forms.Medications.E18_06"
                                                },
                                            new TextBox() { DisplayName = "Route", NgModel = "forms.Medications.E18_04"
                                                },
                                            new TextBox() { DisplayName = "Response", NgModel = "forms.Medications.E18_07"
                                                },
                                            new TextBox() { DisplayName = "Complication", NgModel = "forms.Medications.E18_08"
                                                },
                                            new TextBox() { DisplayName = "Authorization", NgModel = "forms.Medications.E18_10"
                                                },
                                            new TextBox() { DisplayName = "Auth Physician", NgModel = "forms.Medications.E18_11"
                                                },
                                            new TextBox() { DisplayName = "Is Prior Aid", NgModel = "forms.Medications.E18_02"
                                                },
                                            new TextBox() { DisplayName = "Notes", NgModel = "forms.Medications.M3"
                                                },



                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                            NgSubmitClick = "AddItemToList('Medications');",
                                            NgCancelClick = "ClearCloseModal('#Medications', 'Medications');"
                                },

                                ProceduresDialog = new Dialog()
                                {
                                        DialogTargetId = "Procedures",
                                        DialogTitle = "Procedures",
                                        NgFormName = "Procedures",
                                         AllowKeepOpen = true,
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Procedures.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Time", NgModel = "forms.Procedures.P1"

                                                },
                                            new TextBox() { DisplayName = "Date", NgModel = "forms.Procedures.P2"

                                                },
                                            new TextBox() { DisplayName = "Crew", NgModel = "forms.Procedures.E19_09"

                                                },
                                            new TextBox() { DisplayName = "Procedure", NgModel = "forms.Procedures.E19_03"

                                                },
                                            new TextBox() { DisplayName = "Equip Size", NgModel = "forms.Procedures.E19_04"

                                                },
                                            new TextBox() { DisplayName = "Success", NgModel = "forms.Procedures.E19_06"

                                                },
                                            new TextBox() { DisplayName = "Attempts", NgModel = "forms.Procedures.E19_05"

                                                },
                                            new TextBox() { DisplayName = "Response", NgModel = "forms.Procedures.E19_08"

                                                },
                                            new TextBox() { DisplayName = "Complication", NgModel = "forms.Procedures.E19_07"

                                                },
                                            new TextBox() { DisplayName = "Authorization", NgModel = "forms.Procedures.E19_10"

                                                },
                                            new TextBox() { DisplayName = "Auth Phys.", NgModel = "forms.Procedures.E19_11"

                                                },
                                            new TextBox() { DisplayName = "Is Prior Aid", NgModel = "forms.Procedures.E19_02"

                                                },
                                            new TextBox() { DisplayName = "IV Success", NgModel = "forms.Procedures.E19_14"

                                                },
                                            new TextBox() { DisplayName = "Tube Confirmation", NgModel = "forms.Procedures.E19_13"

                                                },
                                            new TextBox() { DisplayName = "Tube Destination", NgModel = "forms.Procedures.E19_14"

                                                },
                                            new TextBox() { DisplayName = "Notes", NgModel = "forms.Procedures.aaa"

                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                            NgSubmitClick = "AddItemToList('Procedures');",
                                            NgCancelClick = "ClearCloseModal('#Procedures', 'Procedures');"
                                },

                                ExamsDialog = new DialogExams()
                                {
                                        DialogTargetId = "Exams",
                                        DialogTitle = "Exams",
                                        NgFormName = "Exams",
                                         AllowKeepOpen = true,
                                          PartialTemplateName = "DialogExams",

                                    Tab1Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Exams.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new DropDownList() { DisplayName = "Head", NgModel = "forms.Exams.E15_02",
                                                DropDownOptions = options.NemsisSelectOptions("E15_02"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Mental", NgModel = "forms.Exams.E16_23",
                                                DropDownOptions = options.NemsisSelectOptions("E16_23"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Neuro", NgModel = "forms.Exams.E16_24",
                                                DropDownOptions = options.NemsisSelectOptions("E16_24"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Face", NgModel = "forms.Exams.E16_05",
                                                DropDownOptions = options.NemsisSelectOptions("E16_05"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Left Eye", NgModel = "forms.Exams.E16_21",
                                                DropDownOptions = options.NemsisSelectOptions("E16_21"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Right Eye", NgModel = "forms.Exams.E16_22",
                                                DropDownOptions = options.NemsisSelectOptions("E16_22"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Neck", NgModel = "forms.Exams.E16_06",
                                                DropDownOptions = options.NemsisSelectOptions("E16_06"), ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Skin", NgModel = "forms.Exams.E16_04",
                                                DropDownOptions = options.NemsisSelectOptions("E16_04"), ResponsiveWidth = 12
                                                }

                                        },


                                        Tab2Controls = new List<Ctrl>()
                                        {
                                             new TextBox() { DisplayName = "Chest", NgModel = "forms.Exams.E16_07",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Heart", NgModel = "forms.Exams.E16_08",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Abs Left Upper", NgModel = "forms.Exams.E16_09",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Abs Left Lower", NgModel = "forms.Exams.E16_10",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Abs Right Upper", NgModel = "forms.Exams.E16_11",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Abs Right Lower", NgModel = "forms.Exams.E16_12",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "GU", NgModel = "forms.Exams.E16_13",
                                                 ResponsiveWidth = 12
                                                }

                                        },

                                        Tab3Controls = new List<Ctrl>()
                                        {
                                             new TextBox() { DisplayName = "Right - Upper", NgModel = "forms.Exams.E16_17",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Right Lower", NgModel = "forms.Exams.E16_18",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Left Lower", NgModel = "forms.Exams.E16_20",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Left Upper", NgModel = "forms.Exams.E16_19",
                                                 ResponsiveWidth = 12
                                                }

                                        },

                                        Tab4Controls = new List<Ctrl>()
                                        {
                                             new TextBox() { DisplayName = "Back Cervical", NgModel = "forms.Exams.E16_14",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Back Thoracic", NgModel = "forms.Exams.E16_15",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Back Lumbar", NgModel = "forms.Exams.E16_16",
                                                 ResponsiveWidth = 12
                                                },
                                             new TextBox() { DisplayName = "Unspecified", NgModel = "forms.Exams.E15_11",
                                                 ResponsiveWidth = 12
                                                }

                                        },

                                        Tab5Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Time", NgModel = "forms.Exams.aaa",
                                                 ResponsiveWidth = 12
                                                },
                                            new TextBox() { DisplayName = "Date", NgModel = "forms.Exams.aaa",
                                                 ResponsiveWidth = 12
                                                },
                                            new TextBox() { DisplayName = "Notes", NgModel = "forms.Exams.CustomNotes",
                                                 ResponsiveWidth = 12
                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                            NgSubmitClick = "AddItemToList('Exams');",
                                            NgCancelClick = "ClearCloseModal('#Exams', 'Exams');"
                                },




                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "timelines",
                                        ngFieldNames =  new List<string>() { "Time", "Group", "Policy" },
                                        DisplayNames = new List<string>() { "Time", "Group", "Policy" }
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
                                    new TextArea() { DisplayName = "", HeightRows = 20
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
