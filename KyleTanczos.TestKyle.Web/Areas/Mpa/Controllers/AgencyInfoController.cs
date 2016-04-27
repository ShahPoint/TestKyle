using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KyleTanczos.TestKyle.Web.Areas.Mpa.Controllers
{
    public class AgencyInfoController : Controller
    {
        // GET: Mpa/AgencyInfo
        public ActionResult Index()
        {
            string state = "PA"; // set from user info
            string agencyToken = "Superior"; // set from user info

            GetPcrFormSelect2Options options = new GetPcrFormSelect2Options(state, agencyToken);
            ConfigurationPage model = new ConfigurationPage();
            model.WebServiceName = "agencyInfo";
            model.ConfigurationName = "AgencyInfo";
            model.Tab = new Tab()
            {
                PartialTemplateName = "TabSingleColumn",
                TabTargetName = "AgencyInfoTab",
                Sections = new List<Section>()
                        {
                            new Section()
                            {

                                SectionName = "Agency Configs",
                                Controls = new List<Ctrl>()
                                {
                                   new TextBox() { DisplayName = "Agency Name"
                                       ,NgModel = "AgencyInfo.agencyName"//, ControlCustomCssClass = "input-spinner"
                                        },
                                   new TextBox() { DisplayName = "Agency Number",
                                        NgModel = "AgencyInfo.agencyNumber"
                                        },
                                    new TextBox() { DisplayName = "OrganizationType"                                       
                                        , NgModel = "AgencyInfo.organizationType"
                                        },
                                    new TextBox() { DisplayName = "Organization Status"
                                        ,NgModel = "AgencyInfo.organizationStatus"
                                        },
                                   new TextBox() { DisplayName = "States Served"
                                        ,NgModel = "AgencyInfo.states"
                                        },
                                     new TextBox() { DisplayName = "Counties Served"
                                       ,NgModel = "AgencyInfo.counties"//, ControlCustomCssClass = "input-spinner"
                                        },
                                    new TextBox() { DisplayName = "TimeZone"
                                    ,NgModel = "AgencyInfo.timeZone"//, ControlCustomCssClass = "input-spinner"
                                    },
                                        new TextBox() { DisplayName = "Level Of Service"
                                    ,NgModel = "AgencyInfo.levelOfService"//, ControlCustomCssClass = "input-spinner"
                                    },  new TextBox() { DisplayName = "Primary Type Of Service"
                                    ,NgModel = "AgencyInfo.primaryTypeOfService"//, ControlCustomCssClass = "input-spinner"
                                    },
                                        new TextBox() { DisplayName = "Other Types Of Service"
                                    ,NgModel = "AgencyInfo.otherTypesOfService"//, ControlCustomCssClass = "input-spinner"
                                    },
                                        new TextBox() { DisplayName = "National Provider Identifier"
                                    ,NgModel = "AgencyInfo.nationalProviderIdentifier"//, ControlCustomCssClass = "input-spinner"
                                    },
                                        new TextBox() { DisplayName = "Agency Contact Zip"
                                    ,NgModel = "AgencyInfo.agencyContactZip"//, ControlCustomCssClass = "input-spinner"
                                    },
                                        new TextBox() { DisplayName = "Zone Numbers"
                                    ,NgModel = "AgencyInfo.zoneNumbers"//, ControlCustomCssClass = "input-spinner"
                                    },

                                }
                            }
                }

            };
            return View(model);
        }

        // GET: Mpa/AgencyInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mpa/AgencyInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mpa/AgencyInfo/Create
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

        // GET: Mpa/AgencyInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mpa/AgencyInfo/Edit/5
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

        // GET: Mpa/AgencyInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Mpa/AgencyInfo/Delete/5
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
