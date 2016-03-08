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
    }

    public enum SectionSideEnum {left, right}

    
    public class Ctrl
    {
        public Ctrl()
        {
            ResponsiveWidth = 6;
            DropDownOptions = new List<Select2Option>();
        }
        public string DisplayName { get; set; }
        public ControlTypeEnum ControlType { get; set; }
        public List<Select2Option> DropDownOptions { get; set; }
        public int ResponsiveWidth { get; set; }
        public string PlaceHolder { get; set; }
        public string ClientId { get; set; }
        public string NgModel { get; set; }



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


    public enum ControlTypeEnum { PatientMeds, MileageBox, TextBox, DropDownList, Select2, Select2Single, Select2Many, Select2TagsSingle, Select2TagsMany, TimePicker }

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

        AppContextDb db = new AppContextDb();
        
        string agencyToken;

        string state;

        List<Select2OptionsList> agencySelect2OptionsLists = null;

        // GET: Mpa/PcrForm
        public ActionResult Index()
        {
            //(Constructor)

            state = "PA"; // set from user info
            agencyToken = "Superior"; // set from user info
            agencySelect2OptionsLists =  db.Select2OptionsList.Where(x =>  x.Association == agencyToken).ToList();

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
                                 SectionName = "Disposition",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Disposition/Outcome", ControlType = ControlTypeEnum.Select2,
                                        DropDownOptions = NemsisSelectOptions("E20_10"), ResponsiveWidth = 12
                                        }

                                }
                            },
                            new Section()
                            {    SectionName = "Incident",
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Incident Number", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Response Urgency", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "CMS Level", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Type Of Location", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Nature Of Incident", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Scene Address", ControlType = ControlTypeEnum.TextBox
                                        }

                                }
                            },
                            new Section()
                            {    SectionName = "Dispatch",
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Call Sign", ControlType = ControlTypeEnum.TextBox
                                        },
                                        new Ctrl() { DisplayName = "Vehicle Number", ControlType = ControlTypeEnum.TextBox
                                        },

                                    new Ctrl() { DisplayName = "Other Agencies", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Mode To Scene", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Responder Time", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Service Requested", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = NemsisSelectOptions("E1_05")
                                        },
                                    new Ctrl() { DisplayName = "Responder Time", ControlType = ControlTypeEnum.TextBox
                                    }
                                }
                            },
                            new Section()
                            {   Side = SectionSideEnum.right,
                                SectionName = "Times",
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Onset", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Recieved", ControlType = ControlTypeEnum.TimePicker,  ResponsiveWidth = 12
                                        },
                                   new Ctrl() { DisplayName = "Notified", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Dispatched", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                     new Ctrl() { DisplayName = "Enroute", ControlType = ControlTypeEnum.TimePicker, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Arrival", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Contacted", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Departed", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Arrival", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Available", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "At Base", ControlType = ControlTypeEnum.TimePicker,ResponsiveWidth = 12
                                        },
                                }
                            },
                            new Section()
                            {
                                SectionName = "Crew",
                                Side = SectionSideEnum.right,
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Primary", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Secondary", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Third", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Other", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        }
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
                                        },
                                    new Ctrl() { DisplayName = "Scene", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Dest.", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        },
                                    new Ctrl() { DisplayName = "Service", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                        }
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
                                       },
                                    new Ctrl() { DisplayName = "Last Name", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "M.I.", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "DOB", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Phone", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Weight", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Gender", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "SSN", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Pattient Address", ControlType = ControlTypeEnum.TextBox
                                       }
                                    }
                             },
                             new Section()
                             {
                                 SectionName = "Personal",
                                 Controls = new List<Ctrl>()
                                 {

                                    new Ctrl() { DisplayName = "DL Number", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "DL State", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Pt Practitioner Name", ControlType = ControlTypeEnum.TextBox
                                       }
                                 }
                             },
                             new Section()
                             {
                                 SectionName = "Medical Info",
                                 Side = SectionSideEnum.right,
                                 Controls = new List<Ctrl>()
                                 {
                                     new Ctrl() { DisplayName = "History", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "History Obtained", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Allergies", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Emergency Form", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Enviromental/Food Allergies", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Advanced Directives", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Triage Color", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Triage Category", ControlType = ControlTypeEnum.TextBox
                                       }
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
                                       },
                                    new Ctrl() { DisplayName = "Duration", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 3
                                       },
                                    new Ctrl() { DisplayName = "Units", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 3
                                       },
                                    new Ctrl() { DisplayName = "Secondary Complaint", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Duration", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 3
                                       },
                                    new Ctrl() { DisplayName = "Units", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 3
                                       },
                                    new Ctrl() { DisplayName = "Barriers To Patient Care", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                       }
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
                            {
                                SectionName = "Vehicle Collision",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Vehicle Collision Impact", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                       },
                                    new Ctrl() { DisplayName = "Report Number", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Pt Location", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Row", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Safety Equipment", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Airbags", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Injury Indicators", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Trauma",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Height of Fall(ft.)", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Cause of Injury", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Intent of Injury", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Trauma Type", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Cardiac Arrest",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Pre-Arrival", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Cardiac Arest Time", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Etiology", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Witnessed By", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "First Rhythm", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Circulation Return", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Resusitations", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Discontinue Reason", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Discontinue Time", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Discontinue Date", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Impression",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Complaint Location", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Organ System", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Primary Symptoms", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Other Symptoms", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Impression", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Secondary Impression", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Drugs/Alcohol", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },

                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Prior Aid Given",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Prior Aid", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Treated By", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Aid Outcome", ControlType = ControlTypeEnum.TextBox
                                       }
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
                                    new Ctrl() { DisplayName = "Condition Codes", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Payment Method", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Necessity Certificate", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Patient Email", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Employer",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Work Related", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Employer", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Employer Address", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Employer Phone", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Patient Occupation", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Occupation Industry", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            }
                            ,new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Guardian/Patient",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Last Name", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "First Name", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "M.I.", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Phone #", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Relationship", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Guardian Address", ControlType = ControlTypeEnum.TextBox
                                       }
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
                                    new Ctrl() { DisplayName = "Destination", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Reason", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Address", ControlType = ControlTypeEnum.TextBox, ResponsiveWidth = 12
                                       },
                                    new Ctrl() { DisplayName = "Transfer Condition", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Type", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Code", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },
                            new Section()
                            {
                                SectionName = "Transport Information",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "MCI", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Rythm at Destination", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Number of Patients", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "ER Disposition", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "To Ambulance Via", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Hospital Disposition", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Transport Position", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "From Ambulance Via", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Mode From Scene", ControlType = ControlTypeEnum.TextBox
                                       }
                                }
                            },

                            new Section()
                            {
                                Side = SectionSideEnum.right,
                                SectionName = "Other Reporting Information",
                                Controls = new List<Ctrl>()
                                {
                                    new Ctrl() { DisplayName = "Discharge Neuro", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Transfer-To Record #", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Trauma Registry ID", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Record #", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Fire Report #", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Zone", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Patient ID Tag #", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Vehicle Lat GPS", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Lat GPS", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Vehicle Long GPS", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Destination Long GPS", ControlType = ControlTypeEnum.TextBox
                                       }
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
                                PartialTemplateName = "SectionWithDialog",
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
                                    new Ctrl() { DisplayName = "Review Requested", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "EMS Injury", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Injury Type", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Contact Blood/Fluids", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Fluid Exposure Type", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Personnel Exposed", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Req. Reportable Cond.", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Registry Candidate", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Protective Equipment", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Disasters", ControlType = ControlTypeEnum.TextBox
                                       },
                                    new Ctrl() { DisplayName = "Precautions", ControlType = ControlTypeEnum.TextBox
                                       }
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

        private List<Select2Option> GetSelect2ListFromDb(string nemsisCode, string state)
        {
            return db.NemsisDataElements.Where(x => x.State == state && x.FieldNumber == nemsisCode).Select(x => new Select2Option() { id = x.OptionText, text = x.OptionText }).ToList();
        }

        /// <summary>
        /// Returns select2 options list based on NemsisCode, global class variables select2OPtionsLists, agencyToken, and state must be preset
        /// </summary>
        /// <param name="NemsisId"></param>
        /// <returns></returns>
        private List<Select2Option> NemsisSelectOptions(string NemsisId)
        {
            var agencySelect2Options = agencySelect2OptionsLists.FirstOrDefault(x => x.ControlName == NemsisId);

            if (agencySelect2Options != null)
            {
                var optionsAsJson = agencySelect2Options.OptionsAsJson;
                return JsonConvert.DeserializeObject<List<Select2Option>>(optionsAsJson); 
             }

            var stateSelect2Options = GetSelect2ListFromDb( NemsisId, "PA");

            if (stateSelect2Options.Count > 0)
            {
                return stateSelect2Options;
            }

            var defaultSelect2Options = GetSelect2ListFromDb( NemsisId, "Default");
            return defaultSelect2Options;
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
