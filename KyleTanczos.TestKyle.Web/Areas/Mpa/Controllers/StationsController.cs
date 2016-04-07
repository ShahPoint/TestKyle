using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class Stations
    {
        public Stations()
        {
            Tab = new Tab();
        }

        public Tab Tab { get; set; }
    }

    public class StationsController : Controller
    {
        

        // GET: Mpa/Stations
        public ActionResult Index()
        {
            //(Constructor)

            string state = "PA"; // set from user info
            string agencyToken = "Superior"; // set from user info

            GetPcrFormSelect2Options options = new GetPcrFormSelect2Options(state, agencyToken);
            Stations model = new Stations();
            model.Tab = new Tab()
            {
                TabTargetName = "PatientTab",
                Sections = new List<Section>()
                         {
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
                                            new DropDownList() { DisplayName = "Immunization Year", NgModel = "forms.Immunizations.E12_13"
                                            , ResponsiveWidth = 12
                                                },
                                            new DropDownList() { DisplayName = "Immunization Type", NgModel = "forms.Immunizations.E12_12",
                                                DropDownOptions = options.NemsisSelectOptions("E12_12"), ResponsiveWidth = 12
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
                                        ngFieldNames =  new List<string>() { "E12_13", "E12_12" },
                                        DisplayNames = new List<string>() { "Year", "Type" }
                                    }
                                }
                            }
                    }

            };
            return View(model);
        }

        // GET: Mpa/Stations/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/Stations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/Stations/Create
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

        // GET: Mpa/Stations/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/Stations/Edit/5
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

        // GET: Mpa/Stations/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/Stations/Delete/5
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
