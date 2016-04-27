using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class VehiclesController : Controller
    {
        // GET: Mpa/Vehicles
        public ActionResult Index()
        {
            string state = "PA"; // set from user info
            string agencyToken = "Superior"; // set from user info

            GetPcrFormSelect2Options options = new GetPcrFormSelect2Options(state, agencyToken);
            ConfigurationPage model = new ConfigurationPage();
            model.WebServiceName = "vehicles";
            model.ConfigurationName = "Vehicles";
            model.Tab = new Tab()
            {
                PartialTemplateName = "TabSingleColumn",
                TabTargetName = "VehiclesTab",
                Sections = new List<Section>()
                         {
                            new Section()
                            {
                                ResponsiveWidth = 12,
                                PartialTemplateName = "SectionWithDialog",
                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "Vehicles",
                                        DialogTitle = "Vehicles",
                                        NgFormName = "Vehicles",
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Vehicles.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Vehicle Number", NgModel = "forms.Vehicles.vehicleNumber"
                                            , ResponsiveWidth = 12
                                            },
                                             new TextBox() { DisplayName = "Call Sign", NgModel = "forms.Vehicles.callSign"
                                            , ResponsiveWidth = 12
                                            }
                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                        NgSubmitClick = "AddItemToList('Vehicles');",
                                        NgCancelClick = "ClearCloseModal('#Vehicles, 'Vehicles');"
                                },
                                SectionName = "Vehicles",
                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "Vehicles",
                                        ngFieldNames =  new List<string>() { "vehicleNumber", "callSign" },
                                        DisplayNames = new List<string>() { "Vehicle Number", "Call Sign"},
                                        ResponsiveWidth = 12
                                    }
                                }
                            }
                    }

            };
            return View(model);
        }

        // GET: Mpa/Vehicles/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/Vehicles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/Vehicles/Create
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

        // GET: Mpa/Vehicles/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/Vehicles/Edit/5
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

        // GET: Mpa/Vehicles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/Vehicles/Delete/5
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
