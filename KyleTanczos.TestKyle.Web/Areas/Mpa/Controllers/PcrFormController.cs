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
            Sections = new List<Section>();
        }
        public string TabTargetName { get; set; }
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        public Section()
        {
            Controls = new List<Ctrl>();
            ResponsiveWidth = 12;
            side = SectionSideEnum.left;
        }
        public string SectionName { get; set; }
        public int ResponsiveWidth { get; set; }
        public List<Ctrl> Controls { get; set; }
        public SectionSideEnum side { get; set; }
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
        
        string agencyToken = "Superior";

        string state = "PA";

        // GET: Mpa/PcrForm
        public ActionResult Index()
        {
            


            var select2OptionsLists =  db.Select2OptionsList.Where(x => x.Association == "Default" || x.Association == state || x.Association == agencyToken).ToList();



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
                                        DropDownOptions = GetSelect2ListFromDb("E20_10"), ResponsiveWidth = 12
                                        }

                                }
                            },
                            new Section()
                            {    SectionName = "Incident",
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Incident Number", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Response Urgency", ControlType = ControlTypeEnum.Select2,
                                        DropDownOptions = GetSelect2Options("E1_04", agencyToken, state, select2OptionsLists)
                                        },

                                    new Ctrl() { DisplayName = "CMS Level", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_03", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Type Of Location", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_01", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Nature Of Incident", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_02", agencyToken, state, select2OptionsLists)
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
                                    new Ctrl() { DisplayName = "Vehicle Number", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_04", agencyToken, state, select2OptionsLists)
                                        },

                                    new Ctrl() { DisplayName = "Other Agencies", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_03", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Mode To Scene", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_01", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Responder Time", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_02", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Service Requested", ControlType = ControlTypeEnum.TextBox
                                        },
                                    new Ctrl() { DisplayName = "Responder Time", ControlType = ControlTypeEnum.DropDownList,
                                        DropDownOptions = GetSelect2Options("E1_02", agencyToken, state, select2OptionsLists)
                                        }
                                }
                            },
                            new Section()
                            {   side = SectionSideEnum.right,
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
                                side = SectionSideEnum.right,
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Primary", ControlType = ControlTypeEnum.Select2, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_04", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Secondary", ControlType = ControlTypeEnum.Select2, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_04", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Third", ControlType = ControlTypeEnum.Select2, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_03", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Other", ControlType = ControlTypeEnum.Select2, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_01", agencyToken, state, select2OptionsLists)
                                        }
                                }
                            },                           
                            new Section()
                            {
                                SectionName = "Mileage",
                                side = SectionSideEnum.right,
                                ResponsiveWidth = 6,
                                Controls = new List<Ctrl>()
                                {
                                   new Ctrl() { DisplayName = "Start", ControlType = ControlTypeEnum.MileageBox, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_04", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Scene", ControlType = ControlTypeEnum.MileageBox, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_04", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Dest.", ControlType = ControlTypeEnum.MileageBox, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_03", agencyToken, state, select2OptionsLists)
                                        },
                                    new Ctrl() { DisplayName = "Service", ControlType = ControlTypeEnum.MileageBox, ResponsiveWidth = 12,
                                        DropDownOptions = GetSelect2Options("E1_01", agencyToken, state, select2OptionsLists)
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
                                 side = SectionSideEnum.right,
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
                                 side = SectionSideEnum.right,
                                 Controls = new List<Ctrl>()
                                 {
                                     new Ctrl()
                                     {
                                        ControlType = ControlTypeEnum.PatientMeds
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

        private List<Select2Option> GetSelect2ListFromDb(string nemsisCode)
        {
            return db.NemsisDataElements.Where(x => x.FieldNumber == nemsisCode).Select(x => new Select2Option() { id = x.OptionText, text = x.OptionText }).ToList();
        }

        private List<Select2Option> GetSelect2Options(string controlName, string agencyToken, string state, List<Select2OptionsList> select2OptionsLists)
        {
            var controlMatchedList = select2OptionsLists.Where(x => x.ControlName == controlName);

            if (controlMatchedList.FirstOrDefault(x => x.Association == agencyToken) != null)
            {
                var optionsAsJson = controlMatchedList.FirstOrDefault(x => x.Association == agencyToken).OptionsAsJson;
                return JsonConvert.DeserializeObject<List<Select2Option>>(optionsAsJson); 
             }

            if (controlMatchedList.FirstOrDefault(x => x.Association == state) != null)
            {
                var optionsAsJson = controlMatchedList.FirstOrDefault(x => x.Association == state).OptionsAsJson;
                return JsonConvert.DeserializeObject<List<Select2Option>>(optionsAsJson);
            }

            var optionsAsJson2 = controlMatchedList.FirstOrDefault(x => x.Association == "Default").OptionsAsJson;
            return JsonConvert.DeserializeObject<List<Select2Option>>(optionsAsJson2);
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
