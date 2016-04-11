using KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{

    public class Medications
    {
        public Medications()
        {
            Tab = new Tab();
        }

        public Tab Tab { get; set; }
    }
    public class MedicationsController : Controller
    {
        // GET: Mpa/Medications
        public ActionResult Index()
        {
            //(Constructor)

            string state = "PA"; // set from user info
            string agencyToken = "Superior"; // set from user info

            GetPcrFormSelect2Options options = new GetPcrFormSelect2Options(state, agencyToken);
            Medications model = new Medications();
            model.Tab = new Tab()
            {
                PartialTemplateName = "TabSingleColumn",
                TabTargetName = "MedicationsTab",
                Sections = new List<Section>()
                         {
                            new Section()
                            {
                                ResponsiveWidth = 12,
                                PartialTemplateName = "SectionWithDialog",
                                Dialog = new Dialog()
                                {
                                        DialogTargetId = "Medications",
                                        DialogTitle = "Medications",
                                        NgFormName = "Medications",
                                        Controls = new List<Ctrl>()
                                        {
                                            new TextBox() { DisplayName = "Hidden Item Index Id", NgModel = "forms.Medications.ItemIndex",
                                                 ResponsiveWidth = 12, ContainerCustomCssClass = "hidden"
                                                },
                                            new TextBox() { DisplayName = "Name", NgModel = "forms.Medications.name"
                                            , ResponsiveWidth = 12
                                                },
                                            new TextBox() { DisplayName = "Certification Level", NgModel = "forms.Medications.certificationLevel",
                                                 ResponsiveWidth = 12
                                                }

                                        },
                                            //OnCancelClick = "alert('cancel')",
                                            //OnSubmitClick = "alert('submit')",
                                        NgSubmitClick = "AddItemToList('Medications');",
                                        NgCancelClick = "ClearCloseModal('#Medications', 'Medications');"
                                },
                                SectionName = "Medications",
                                Controls = new List<Ctrl>()
                                {
                                    new TableListView() {
                                        ngListName = "Medications",
                                        ngFieldNames =  new List<string>() { "name", "certificationLevel" },
                                        DisplayNames = new List<string>() { "Name", "Certification Level" },
                                        ResponsiveWidth = 12
                                    }
                                }
                            }
                    }

            };
            return View(model);
        }

        // GET: Mpa/Medications/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/Medications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/Medications/Create
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

        // GET: Mpa/Medications/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/Medications/Edit/5
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

        // GET: Mpa/Medications/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/Medications/Delete/5
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
