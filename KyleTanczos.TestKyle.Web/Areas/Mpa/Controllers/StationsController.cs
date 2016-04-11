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
                TabTargetName = "StationsTab",
                Sections = new List<Section>()
                         {
                            new Section()
                            {
                                ResponsiveWidth = 12,
                                PartialTemplateName = "SectionWithDialog",
                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "Stations",
                                        DialogTitle = "Stations",
                                        NgFormName = "Stations",
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Stations.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Name", NgModel = "forms.Stations.name"
                                            , ResponsiveWidth = 12
                                                },
                                            new TextBox() { DisplayName = "Number", NgModel = "forms.Stations.number",
                                                 ResponsiveWidth = 12
                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                        NgSubmitClick = "AddItemToList('Stations');",
                                        NgCancelClick = "ClearCloseModal('#Stations', 'Stations');"
                                },
                                SectionName = "Stations",                            
                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "Stations",
                                        ngFieldNames =  new List<string>() { "name", "number" },
                                        DisplayNames = new List<string>() { "Name", "Number" },
                                        ResponsiveWidth = 12
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
